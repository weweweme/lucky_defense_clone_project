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
        /// 해당 유닛의 최상위 컨테이너인 UnitRoot 참조
        /// </summary>
        private UnitRoot _unitRoot;

        /// <summary>
        /// 컴포넌트 초기화 시 SpriteRenderer 컴포넌트를 필수로 가져옵니다.
        /// 존재하지 않을 경우 예외를 발생시켜 문제 상황을 조기에 인지할 수 있도록 합니다.
        /// </summary>
        private void Awake()
        {
            AssertHelper.NotNull(typeof(UnitSpriteController), _spriteRenderer);
        }

        /// <summary>
        /// 유닛 생성 시점에 UnitRoot 정보를 주입받아 내부 참조를 설정합니다.
        /// 이를 통해, 유닛의 등급과 타입 정보를 기반으로 적절한 Sprite를 가져올 수 있습니다.
        /// </summary>
        /// <param name="root">해당 유닛의 UnitRoot 참조</param>
        public void Init(UnitRoot root)
        {
            _unitRoot = root;
        }

        /// <summary>
        /// 유닛의 등급과 타입에 맞는 스프라이트 및 크기 정보를 조회하여 실제 적용합니다.
        /// 유닛 외형을 현재 상태에 맞게 갱신하는 역할을 합니다.
        /// </summary>
        public void ChangeVisible()
        {
            UnitMetaData metaData = _unitRoot.dependencyContainer.mdlUnitResources.GetResource(_unitRoot.grade, _unitRoot.type);
            AssertHelper.NotNull(typeof(UnitAttackController), metaData);
            
            _spriteRenderer.sprite = metaData.Sprite;
            _spriteRenderer.transform.localScale = Vector3.one * metaData.ScaleSize;
        }
    }
}
