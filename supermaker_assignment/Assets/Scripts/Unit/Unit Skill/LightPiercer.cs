using System.Threading;
using Cysharp.Threading.Tasks;
using Enemy;
using UnityEngine;
using Util;

namespace Unit.Unit_Skill
{
    /// <summary>
    /// 희귀 원거리 유닛의 스킬입니다.
    /// </summary>
    public sealed class LightPiercer : UnitSkillBase
    {
        [SerializeField] private float _flightDuration = 0.5f;
        [SerializeField] private CircleCollider2D _explosionRange;
        [SerializeField] private uint _damage = 10;
        [SerializeField] private ParticleSystem _explosionEffect;
        
        private readonly LayerMask _enemyLayer = Layers.GetLayerMask(Layers.ENEMY);
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly Collider2D[] _hitEnemiesBuffer = new Collider2D[16];

        private void Awake()
        {
            AssertHelper.NotNull(typeof(LightPiercer), _explosionRange);
            AssertHelper.NotNull(typeof(LightPiercer), _explosionEffect);
        }

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

            _explosionEffect.transform.localScale = Vector3.one * 0.08f;
            _explosionEffect.Play();
            
            // 1. Duration동안 startTr에서 targetTr로 이동
            while (elapsedTime < _flightDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / _flightDuration;
                transform.position = Vector3.Lerp(startPosition, targetPosition, t);

                await UniTask.Yield(_cts.Token);
            }

            transform.position = targetPosition;

            // 2. 폭발
            _explosionEffect.transform.localScale = Vector3.one * 0.28f;
            
            // 3. 원형 감지 후 데미지 적용
            ApplyExplosionDamage(targetPosition);
            await UniTask.Delay(Mathf.RoundToInt(_explosionEffect.main.duration * 1000), cancellationToken: _cts.Token);

            // 4. 스킬 오브젝트 풀로 반환
            _explosionEffect.Stop();
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
