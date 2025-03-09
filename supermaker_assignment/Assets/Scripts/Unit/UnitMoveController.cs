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

        private void Awake()
        {
            AssertHelper.NotNull(typeof(UnitMoveController), _moveEffect);
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
