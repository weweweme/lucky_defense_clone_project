using Model;
using UnityEngine;
using Util;

namespace Unit
{
    /// <summary>
    /// 유닛의 Sprite 및 시각적 요소 처리를 담당하는 컨트롤러 클래스입니다.
    /// </summary>
    public sealed class UnitSpriteController : MonoBehaviourBase
    {
        /// <summary>
        /// 유닛의 스프라이트 렌더링을 담당하는 컴포넌트
        /// </summary>
        [SerializeField] private SpriteRenderer _spriteRenderer;

        /// <summary>
        /// 유닛의 애니메이션 처리를 담당하는 컴포넌트 
        /// </summary>
        [SerializeField] private Animator _animator;

        /// <summary>
        /// 해당 유닛의 최상위 컨테이너인 UnitRoot 참조
        /// </summary>
        private UnitRoot _unitRoot;
        
        /// <summary>
        /// 유닛의 등급 및 타입에 따른 리소스 정보를 관리하는 모델 클래스
        /// </summary>
        private MDL_UnitResources _mdl;

        /// <summary>
        /// 어택 애니메이션을 위한 해시값
        /// </summary>
        private static readonly int _isAttackingHash = Animator.StringToHash("isAttacking");

        private void Awake()
        {
            AssertHelper.NotNull(typeof(UnitSpriteController), _spriteRenderer);
            AssertHelper.NotNull(typeof(UnitSpriteController), _animator);
        }

        /// <summary>
        /// 유닛 생성 시점에 UnitRoot 정보를 주입받아 내부 참조를 설정합니다.
        /// </summary>
        /// <param name="root">해당 유닛의 UnitRoot 참조</param>
        public void CreatePooledItemInit(UnitRoot root)
        {
            _unitRoot = root;
            _mdl = root.dependencyContainer.mdlUnitResources;
        }

        /// <summary>
        /// 유닛의 등급과 타입에 맞는 스프라이트 및 크기 정보를 조회하여 실제 적용합니다.
        /// 유닛 외형을 현재 상태에 맞게 갱신하는 역할을 합니다.
        /// </summary>
        public void ChangeVisible()
        {
            UnitMetaData metaData = _mdl.GetResource(_unitRoot.grade, _unitRoot.type);
            AssertHelper.NotNull(typeof(UnitAttackController), metaData);
            
            Material material = _mdl.GetUnitGradeMaterial(_unitRoot.grade);
            AssertHelper.NotNull(typeof(UnitAttackController), material);
            
            _spriteRenderer.sprite = metaData.Sprite;
            _spriteRenderer.material = material;
            _spriteRenderer.transform.localScale = Vector3.one * metaData.ScaleSize;
            _animator.runtimeAnimatorController = metaData.AnimatorController;
        }

        /// <summary>
        /// 현재 어택 애니메이션의 활성화 여부입니다
        /// </summary>
        /// <param name="value"></param>
        public void SetIsAttacking(bool value)
        {
            _animator.SetBool(_isAttackingHash, value);   
        }
        
        /// <summary>
        /// 대상 방향으로 유닛의 좌우 방향을 설정합니다.
        /// </summary>
        public void SetFacingDirection(Transform target)
        {
            AssertHelper.NotNull(typeof(UnitSpriteController), target);
            
            float dirX = target.position.x - transform.position.x;
            float sign = (dirX < 0) ? -1f : 1f;

            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * sign;
            transform.localScale = scale;
        }
    }
}
