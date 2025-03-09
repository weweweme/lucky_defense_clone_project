using System.Threading;
using Cysharp.Threading.Tasks;
using Enemy;
using UnityEngine;
using Util;

namespace Unit.Unit_Skill
{
    /// <summary>
    /// 희귀 원거리 유닛의 스킬입니다.
    /// 지정된 타겟을 향해 0.5초 동안 날아가며, 도착 후 폭발하여 주변 적들에게 피해를 줍니다.
    /// </summary>
    public sealed class LightPiercer : UnitSkillBase
    {
        [SerializeField] private float _flightDuration = 0.5f;
        [SerializeField] private CircleCollider2D _explosionRange;
        [SerializeField] private uint _damage = 10;
        
        private readonly LayerMask _enemyLayer = Layers.GetLayerMask(Layers.ENEMY);
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly Collider2D[] _hitEnemiesBuffer = new Collider2D[16];

        public override void UseSkill()
        {
            ExecuteSkill().Forget();
        }

        private async UniTask ExecuteSkill()
        {
            AssertHelper.NotNull(typeof(LightPiercer), startTr);
            AssertHelper.NotNull(typeof(LightPiercer), targetTr);

            Vector3 startPosition = startTr.position;
            Vector3 targetPosition = targetTr.position;
            float elapsedTime = 0f;

            // 1. Duration동안 startTr에서 targetTr로 이동
            while (elapsedTime < _flightDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / _flightDuration;
                transform.position = Vector3.Lerp(startPosition, targetPosition, t);

                await UniTask.Yield(_cts.Token);
            }

            transform.position = targetPosition;

            // 2. 폭발 (디버그 로그 출력)
            Debug.Log("폭발");
            
            // 3. 원형 감지 후 4. 데미지 적용
            ApplyExplosionDamage(targetPosition);

            // 5. 스킬 오브젝트 풀로 반환
            skillPool.ReleaseObject(this);
        }
        
        private void ApplyExplosionDamage(Vector3 explosionPosition)
        {
            int hitCount = Physics2D.OverlapCircleNonAlloc(explosionPosition, _explosionRange.radius, _hitEnemiesBuffer, _enemyLayer);

            for (int i = 0; i < hitCount; ++i)
            {
                var enemyStat = _hitEnemiesBuffer[i].gameObject.GetComponentOrAssert<EnemyStatController>();
                AssertHelper.NotNull(typeof(LightPiercer), enemyStat);
                
                enemyStat.TakeDamage(_damage);
            }
        }
        
        protected override void OnDestroy()
        {
            CancelTokenHelper.DisposeToken(in _cts);
            
            base.OnDestroy();
        }
    }
}
