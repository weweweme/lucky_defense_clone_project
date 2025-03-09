using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Enemy;
using UnityEngine;
using Util;

namespace Unit.Unit_Skill
{
    /// <summary>
    /// 신화 원거리 유닛의 스킬입니다.
    /// 일정 시간 동안 범위 내 무작위 지점을 지속적으로 공격하며, 적들에게 광역 지속 피해를 줍니다.
    /// </summary>
    public sealed class ShadowAnnihilation : UnitSkillBase
    {
        [SerializeField] private float _effectDuration = 3f;
        [SerializeField] private float _effectInterval = 0.4f;
        [SerializeField] private CircleCollider2D _damageRange;
        [SerializeField] private uint _damage = 20;
        [SerializeField] private List<ParticleSystem> _particleEffects;

        private readonly LayerMask _enemyLayer = Layers.GetLayerMask(Layers.ENEMY);
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly Collider2D[] _hitEnemiesBuffer = new Collider2D[16];

        private void Awake()
        {
            AssertHelper.NotNull(typeof(ShadowAnnihilation), _damageRange);
            AssertHelper.NotNull(typeof(ShadowAnnihilation), _particleEffects);
        }

        public override void UseSkill()
        {
            ExecuteSkill().Forget();
        }

        private async UniTask ExecuteSkill()
        {
            AssertHelper.NotNull(typeof(ShadowAnnihilation), startTr);

            int effectCount = Mathf.FloorToInt(_effectDuration / _effectInterval);
            int particleIndex = 0;

            for (int i = 0; i < effectCount; ++i)
            {
                Vector2 randomOffset = Random.insideUnitCircle * _damageRange.radius;
                Vector3 effectPosition = (Vector2)startTr.position + randomOffset;

                PlayParticleEffect(effectPosition, particleIndex);
                ApplyDamage(effectPosition);

                particleIndex = (particleIndex + 1) % _particleEffects.Count;
                await UniTask.Delay(Mathf.RoundToInt(_effectInterval * 1000), cancellationToken: _cts.Token);
            }

            // 스킬 종료 후 풀 반환
            if (IsOwned()) return;
            SkillRelease();
        }

        /// <summary>
        /// 지정된 위치에서 파티클 효과를 실행합니다.
        /// </summary>
        private void PlayParticleEffect(Vector3 position, int index)
        {
            _particleEffects[index].transform.position = position;
            _particleEffects[index].Play();
        }

        /// <summary>
        /// 지정된 위치에서 범위 내 적들에게 지속 피해를 줍니다.
        /// </summary>
        private void ApplyDamage(Vector3 effectPosition)
        {
            int hitCount = Physics2D.OverlapCircleNonAlloc(effectPosition, 0.5f, _hitEnemiesBuffer, _enemyLayer);

            for (int i = 0; i < hitCount; ++i)
            {
                var enemyStat = _hitEnemiesBuffer[i].gameObject.GetComponentOrAssert<EnemyStatController>();
                AssertHelper.NotNull(typeof(ShadowAnnihilation), enemyStat);
                
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
