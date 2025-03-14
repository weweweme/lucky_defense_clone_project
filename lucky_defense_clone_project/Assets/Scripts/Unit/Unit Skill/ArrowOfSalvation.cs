using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Enemy;
using UnityEngine;
using Util;

namespace Unit.Unit_Skill
{
    /// <summary>
    /// 영웅 원거리 유닛의 스킬입니다.
    /// 지정된 목표 지점으로 날아간 후, 일정 시간 동안 지속적으로 범위 내 공격을 수행합니다.
    /// </summary>
    public sealed class ArrowOfSalvation : UnitSkillBase
    {
        [SerializeField] private float _flightDuration = 0.5f;
        [SerializeField] private float _effectDuration = 3f;
        [SerializeField] private float _effectInterval = 0.3f;
        [SerializeField] private CircleCollider2D _damageRange;
        [SerializeField] private uint _damage = 8;
        [SerializeField] private List<ParticleSystem> _particleSystems;

        private readonly LayerMask _enemyLayer = Layers.GetLayerMask(Layers.ENEMY);
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly Collider2D[] _hitEnemiesBuffer = new Collider2D[16];

        private int _particleIndex = 0;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(ArrowOfSalvation), _damageRange);
            AssertHelper.NotNull(typeof(ArrowOfSalvation), _particleSystems);
            AssertHelper.NotEqualsValue(typeof(ArrowOfSalvation), _particleSystems.Count, 0);
        }

        public override void UseSkill()
        {
            ExecuteSkill().Forget();
        }

        private async UniTask ExecuteSkill()
        {
            AssertHelper.NotNull(typeof(ArrowOfSalvation), startTr);
            AssertHelper.NotNull(typeof(ArrowOfSalvation), targetTr);

            Vector3 startPosition = startTr.position;
            Vector3 targetPosition = targetTr.position;
            float elapsedTime = 0f;

            // 1. 0.5초 동안 타겟 방향으로 이동
            while (elapsedTime < _flightDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / _flightDuration;
                transform.position = Vector3.Lerp(startPosition, targetPosition, t);

                await UniTask.Yield(_cts.Token);
            }

            transform.position = targetPosition;

            // 2. x초 동안 y초 간격으로 범위 내 랜덤한 위치에서 파티클 효과 실행
            int effectCount = Mathf.FloorToInt(_effectDuration / _effectInterval);

            for (int i = 0; i < effectCount; ++i)
            {
                PlayRandomParticle();
                ApplyRandomExplosionDamage();
                await UniTask.Delay(Mathf.RoundToInt(_effectInterval * 1000), cancellationToken: _cts.Token);
            }

            // 3. 스킬 오브젝트 풀로 반환
            if (IsOwned()) return;
            SkillRelease();
        }

        /// <summary>
        /// CircleCollider2D 범위 내 랜덤한 위치에서 파티클 효과 실행
        /// </summary>
        private void PlayRandomParticle()
        {
            if (_particleSystems.Count == 0) return;

            // 현재 사용할 파티클 선택 (순차적으로 실행)
            int particleIdx = _particleIndex % _particleSystems.Count;
            ParticleSystem selectedParticle = _particleSystems[particleIdx];

            Vector2 randomOffset = Random.insideUnitCircle * _damageRange.radius;
            Vector3 effectPosition = (Vector2)transform.position + randomOffset;

            // 선택된 파티클을 해당 위치에서 실행
            selectedParticle.transform.position = effectPosition;
            selectedParticle.Play();

            _particleIndex++; // 다음 파티클로 인덱스 증가
        }

        /// <summary>
        /// 랜덤한 위치에 피해 적용
        /// </summary>
        private void ApplyRandomExplosionDamage()
        {
            Vector2 randomOffset = Random.insideUnitCircle * _damageRange.radius;
            Vector3 explosionPosition = (Vector2)transform.position + randomOffset;

            int hitCount = Physics2D.OverlapCircleNonAlloc(explosionPosition, 0.5f, _hitEnemiesBuffer, _enemyLayer);

            for (int i = 0; i < hitCount; ++i)
            {
                var enemyStat = _hitEnemiesBuffer[i].gameObject.GetComponentOrAssert<EnemyStatController>();
                AssertHelper.NotNull(typeof(ArrowOfSalvation), enemyStat);
                
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
