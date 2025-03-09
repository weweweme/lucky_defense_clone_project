using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Enemy;
using UnityEngine;
using Util;

namespace Unit.Unit_Skill
{
    /// <summary>
    /// 신화 근거리 유닛의 스킬입니다.
    /// 타겟 위치에 강력한 범위 공격을 가하며, 1초 동안 지속적으로 적을 공격합니다.
    /// </summary>
    public sealed class HarlequinOfDoom : UnitSkillBase
    {
        [SerializeField] private float _effectInterval = 0.1f;
        [SerializeField] private int _effectCount = 10;
        [SerializeField] private CircleCollider2D _damageRange;
        [SerializeField] private uint _damagePerTick = 50;
        [SerializeField] private ParticleSystem _initialEffect;
        [SerializeField] private List<ParticleSystem> _secondaryEffects;

        private readonly LayerMask _enemyLayer = Layers.GetLayerMask(Layers.ENEMY);
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly Collider2D[] _hitEnemiesBuffer = new Collider2D[16];

        private void Awake()
        {
            AssertHelper.NotNull(typeof(HarlequinOfDoom), _damageRange);
            AssertHelper.NotNull(typeof(HarlequinOfDoom), _initialEffect);
            AssertHelper.NotNull(typeof(HarlequinOfDoom), _secondaryEffects);
        }

        public override void UseSkill()
        {
            ExecuteSkill().Forget();
        }

        private async UniTask ExecuteSkill()
        {
            AssertHelper.NotNull(typeof(HarlequinOfDoom), targetTr);

            Vector3 targetPosition = targetTr.position;

            // 1. 첫 번째 파티클 1회 실행
            _initialEffect.transform.position = targetPosition;
            _initialEffect.Play();

            // 2. 지속 데미지 + 보조 파티클 실행
            int secondaryEffectIndex = 0;

            for (int i = 0; i < _effectCount; ++i)
            {
                ApplyDamage(targetPosition);
                PlaySecondaryEffect(targetPosition, secondaryEffectIndex);

                secondaryEffectIndex = (secondaryEffectIndex + 1) % _secondaryEffects.Count;
                await UniTask.Delay(Mathf.RoundToInt(_effectInterval * 1000), cancellationToken: _cts.Token);
            }

            // 3. 스킬 오브젝트 풀로 반환
            if (IsOwned()) return;
            SkillRelease();
        }

        /// <summary>
        /// 지정된 위치에서 범위 내 적들에게 지속 피해를 줍니다.
        /// </summary>
        private void ApplyDamage(Vector3 position)
        {
            int hitCount = Physics2D.OverlapCircleNonAlloc(position, _damageRange.radius, _hitEnemiesBuffer, _enemyLayer);

            for (int i = 0; i < hitCount; ++i)
            {
                var enemyStat = _hitEnemiesBuffer[i].gameObject.GetComponentOrAssert<EnemyStatController>();
                AssertHelper.NotNull(typeof(HarlequinOfDoom), enemyStat);

                enemyStat.TakeDamage(_damagePerTick);
            }
        }

        /// <summary>
        /// 지속적으로 발생하는 보조 파티클 효과를 실행합니다.
        /// </summary>
        private void PlaySecondaryEffect(Vector3 position, int index)
        {
            _secondaryEffects[index].transform.position = position;
            _secondaryEffects[index].Play();
        }

        protected override void OnDestroy()
        {
            CancelTokenHelper.DisposeToken(in _cts);
            base.OnDestroy();
        }
    }
}
