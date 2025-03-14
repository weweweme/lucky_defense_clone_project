using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Util;

namespace Unit
{
    /// <summary>
    /// 유닛의 이동 관련 움직임을 담당하는 클래스입니다.
    /// </summary>
    public sealed class UnitMoveController : MonoBehaviourBase
    {
        [SerializeField] private ParticleSystem _moveEffect;
        [SerializeField] private float _moveDuration = 1f;
        private UnitSpriteController _spriteController;
        private UnitAttackController _attackController;
        private UnitRoot _root;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(UnitMoveController), _moveEffect);
        }
        
        /// <summary>
        /// 유닛 생성 시점에 UnitRoot 정보를 주입받아 내부 참조를 설정합니다.
        /// </summary>
        /// <param name="root">해당 유닛의 UnitRoot 참조</param>
        public void CreatePooledItemInit(UnitRoot root)
        {
            _root = root;
            _spriteController = root.spriteController;
            _attackController = root.attackController;
        }

        public void ChangeEffectScale()
        {
            // 이동 효과의 크기 설정
            float scaleValue = (_root.grade == EUnitGrade.Mythic) ? 6f : 2.5f;
            _moveEffect.transform.localScale = Vector3.one * scaleValue;
        }

        /// <summary>
        /// 지정된 위치까지 일정 시간에 걸쳐 이동합니다.
        /// </summary>
        /// <param name="target">도착 지점 Transform</param>
        public async UniTask MoveToTarget(Transform target)
        {
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = target.position;
            float elapsedTime = 0f;

            // 이동 시작 시 어택을 종료하고 방향 조정.
            _attackController.ClearTarget();
            _spriteController.SetFacingDirection(target);
            
            _moveEffect.Play();

            while (elapsedTime < _moveDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / _moveDuration;
                transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                
                await UniTask.Yield();
            }

            transform.position = targetPosition; // 정확한 위치 보정
            _moveEffect.Stop();
        }
    }
}
