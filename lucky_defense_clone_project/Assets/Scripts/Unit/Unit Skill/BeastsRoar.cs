using System.Threading;
using Cysharp.Threading.Tasks;
using Enemy;
using UnityEngine;
using Util;

namespace Unit.Unit_Skill
{
    /// <summary>
    /// 영웅 근거리 유닛의 스킬입니다.
    /// 지정된 목표 지점에서 즉시 폭발하여 범위 내 적들에게 피해를 줍니다.
    /// </summary>
    public sealed class BeastsRoar : UnitSkillBase
    {
        [SerializeField] private ParticleSystem _particleEffect;
        [SerializeField] private CircleCollider2D _damageRange;
        [SerializeField] private uint _damage = 15;

        private readonly LayerMask _enemyLayer = Layers.GetLayerMask(Layers.ENEMY);
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly Collider2D[] _hitEnemiesBuffer = new Collider2D[16];

        private void Awake()
        {
            AssertHelper.NotNull(typeof(BeastsRoar), _particleEffect);
            AssertHelper.NotNull(typeof(BeastsRoar), _damageRange);
        }

        public override void UseSkill()
        {
            ExecuteSkill().Forget();
        }

        private async UniTask ExecuteSkill()
        {
            AssertHelper.NotNull(typeof(BeastsRoar), targetTr);

            // 1. 파티클 이펙트 해당 위치로 세팅
            _particleEffect.transform.position = targetTr.position;

            // 2. 이펙트 실행
            _particleEffect.Play();

            // 3. 원형 범위 내 적에게 데미지 적용
            ApplyExplosionDamage(targetTr.position);

            // 4. 파티클 이펙트가 종료될 때까지 대기
            await UniTask.Delay(Mathf.RoundToInt(_particleEffect.main.duration * 1000), cancellationToken: _cts.Token);

            // 5. 오브젝트 풀 반환
            if (IsOwned()) return;
            SkillRelease();
        }

        /// <summary>
        /// 지정된 위치에서 범위 내 적들에게 피해를 줍니다.
        /// </summary>
        private void ApplyExplosionDamage(Vector3 explosionPosition)
        {
            int hitCount = Physics2D.OverlapCircleNonAlloc(explosionPosition, _damageRange.radius, _hitEnemiesBuffer, _enemyLayer);

            for (int i = 0; i < hitCount; ++i)
            {
                var enemyStat = _hitEnemiesBuffer[i].gameObject.GetComponentOrAssert<EnemyStatController>();
                AssertHelper.NotNull(typeof(BeastsRoar), enemyStat);

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
