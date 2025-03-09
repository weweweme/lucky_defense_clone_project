using System.Threading;
using Cysharp.Threading.Tasks;
using Enemy;
using UnityEngine;
using Util;

namespace Unit.Unit_Skill
{
    /// <summary>
    /// 희귀 근거리 유닛의 스킬입니다.
    /// 강력한 충격파를 일으켜 주변 적들에게 피해를 줍니다.
    /// </summary>
    public sealed class HellForgeSmash : UnitSkillBase
    {
        [SerializeField] private CircleCollider2D _damageRange;
        [SerializeField] private uint _damage = 15;
        [SerializeField] private ParticleSystem _smashEffect;

        private readonly LayerMask _enemyLayer = Layers.GetLayerMask(Layers.ENEMY);
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly Collider2D[] _hitEnemiesBuffer = new Collider2D[16];

        private void Awake()
        {
            AssertHelper.NotNull(typeof(HellForgeSmash), _damageRange);
            AssertHelper.NotNull(typeof(HellForgeSmash), _smashEffect);
        }

        public override void UseSkill()
        {
            ExecuteSkill().Forget();
        }

        private async UniTask ExecuteSkill()
        {
            AssertHelper.NotNull(typeof(HellForgeSmash), startTr);

            transform.position = startTr.position;
            
            // 1. 파티클 이펙트 실행
            _smashEffect.Play();

            // 2. 원형 감지 후 데미지 적용
            ApplySmashDamage(startTr.position);

            // 3. 이펙트 종료 후 반환
            await UniTask.Delay(Mathf.RoundToInt(_smashEffect.main.duration * 1000), cancellationToken: _cts.Token);

            _smashEffect.Stop();

            if (IsOwned()) return;
            SkillRelease();
        }

        private void ApplySmashDamage(Vector3 smashPosition)
        {
            int hitCount = Physics2D.OverlapCircleNonAlloc(smashPosition, _damageRange.radius, _hitEnemiesBuffer, _enemyLayer);

            for (int i = 0; i < hitCount; ++i)
            {
                var enemyStat = _hitEnemiesBuffer[i].gameObject.GetComponentOrAssert<EnemyStatController>();
                AssertHelper.NotNull(typeof(HellForgeSmash), enemyStat);

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
