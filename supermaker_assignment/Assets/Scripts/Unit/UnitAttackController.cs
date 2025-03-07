using System.Threading;
using CleverCrow.Fluid.BTs.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Util;

namespace Unit
{
    public class UnitAttackController : MonoBehaviourBase
    {
        /// <summary>
        /// 타워가 감지할 대상의 레이어 마스크입니다.
        /// </summary>
        private readonly int TARGET_LAYER = Layers.GetLayerMask(Layers.ENEMY);

        /// <summary>
        /// 유닛의 공격을 제어하는 CancellationTokenSource입니다.
        /// </summary>
        private CancellationTokenSource _cts;

        /// <summary>
        /// 현재 타겟으로 설정된 게임 오브젝트입니다.
        /// 공격 대상이 존재하지 않으면 null입니다.
        /// </summary>
        private readonly TargetEnemyData _currentTarget = new TargetEnemyData();

        /// <summary>
        /// 유닛의 공격 범위를 결정하는 CircleCollider2D입니다.
        /// </summary>
        [SerializeField] private CircleCollider2D _attackRange;

        /// <summary>
        /// OverlapCircleNonAlloc에서 사용될 충돌체 버퍼입니다.
        /// </summary>
        private readonly Collider2D[] _hitColsBuffer = new Collider2D[64];

        /// <summary>
        /// 유닛의 공격 속도 (초당 공격 횟수)입니다.
        /// </summary>
        private float _fireRate = float.MaxValue;
        /// <summary>
        /// 유닛의 다음 공격까지 발사까지 남은 쿨다운 시간(초)입니다.
        /// </summary>
        private float _fireCooldown;

        /// <summary>
        /// 유닛이 현재 공격 중인지 여부입니다.
        /// </summary>
        private bool _isAttacking;

        /// <summary>
        /// 유닛의 데미지입니다.
        /// </summary>
        private uint _damage = uint.MaxValue;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(UnitAttackController), _attackRange);
            _attackRange.radius = float.MaxValue;
        }

        public void Init()
        {
            CancelTokenHelper.GetToken(ref _cts);
            StartAttacking(_cts.Token).Forget();
        }

        public void OnTakeFromPoolInit(UnitRoot root)
        {
            UnitMetaData metaData = root.dependencyContainer.mdlUnitResources.GetResource(root.Grade, root.Type);
            AssertHelper.NotNull(typeof(UnitAttackController), metaData);
            
            _fireRate = metaData.AttackFireRate;
            _damage = metaData.AttackDamage;
            _attackRange.radius = metaData.AttackRange;
            
            AssertHelper.NotEqualsValue(typeof(UnitAttackController), _damage, uint.MaxValue);
            AssertHelper.NotEqualsValue(typeof(UnitAttackController), _fireRate, float.MaxValue);
            AssertHelper.NotEqualsValue(typeof(UnitAttackController), _attackRange.radius, float.MaxValue);
        }

        /// <summary>
        /// 현재 타겟이 존재하는지 여부를 반환합니다.
        /// </summary>
        public bool HasTarget() => _currentTarget.HasTarget;

        /// <summary>
        /// 현재 타겟이 사거리 안에 있는지 확인합니다.
        /// </summary>
        /// <returns>타겟이 사거리 안에 있으면 true, 아니면 false</returns>
        public bool IsTargetInRange()
        {
            // 현재 타겟이 없으면 사거리 안에 있을 수 없으므로 false 반환
            if (!_currentTarget.HasTarget) return false;

            // 타워와 타겟 간의 제곱거리 계산
            Vector3 towerPos = transform.position;
            Vector3 targetPos = _currentTarget.Transform.position;
            float distance = Vector3.SqrMagnitude(targetPos - towerPos);

            // 탐색 범위의 제곱과 비교
            float detectRange = _attackRange.radius;
            return distance <= detectRange * detectRange;
        }

        /// <summary>
        /// 공격 범위 내에서 Enemy 레이어에 속한 타겟을 찾습니다.
        /// </summary>
        /// <returns>타겟을 찾으면 TaskStatus.Success, 없으면 TaskStatus.Failure 반환</returns>
        public TaskStatus FindTarget()
        {
            Vector3 towerPos = transform.position;
            float detectRange = _attackRange.radius;

            int hitCount = Physics2D.OverlapCircleNonAlloc(towerPos, detectRange, _hitColsBuffer, TARGET_LAYER);

            if (hitCount == 0)
            {
                return ClearTarget();
            }

            float closestDistance = float.MaxValue;
            Collider2D closestTarget = null;

            for (int i = 0; i < hitCount; ++i)
            {
                Collider2D targetCol = _hitColsBuffer[i];

                float distance = Vector3.SqrMagnitude(targetCol.transform.position - towerPos);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = targetCol;
                }
            }

            if (closestTarget != null)
            {
                _currentTarget.SetTarget(closestTarget.gameObject);
                return TaskStatus.Success;
            }

            return ClearTarget();
        }

        /// <summary>
        /// 현재 타겟을 해제합니다.
        /// </summary>
        public TaskStatus ClearTarget()
        {
            _currentTarget.Clear();
            return TaskStatus.Failure;
        }

        /// <summary>
        /// 타워가 지속적으로 공격을 수행하는 루틴입니다.
        /// 타겟이 존재하면 일정 간격(fireRate)마다 공격하며, FixedUpdate 타이밍에 실행됩니다.
        /// 타워가 배치되면 루프를 시작하고, 비활성화되면 루프를 종료합니다.
        /// </summary>
        /// <param name="token">비동기 작업을 취소할 수 있는 CancellationToken</param>
        private async UniTaskVoid StartAttacking(CancellationToken token)
        {
            while (!token.IsCancellationRequested) // 취소 요청이 들어오면 루프 종료
            {
                // FixedUpdate 타이밍에서 실행되도록 대기
                await UniTask.NextFrame(PlayerLoopTiming.FixedUpdate, token);

                // 쿨다운이 남아 있으면 감소시키기
                if (_fireCooldown > 0)
                {
                    _fireCooldown = Mathf.Max(_fireCooldown - Time.fixedDeltaTime, 0f);
                }

                // 현재 타겟이 없다면 다음 루프로 이동
                if (!_currentTarget.HasTarget) continue;

                // 현재 타겟이 죽었다면 타겟을 비우고 다음 루프로 이동
                if (_currentTarget.IsDead())
                {
                    ClearTarget();
                    continue;
                }

                // 발사 쿨다운이 남아있다면 다음 루프로 이동
                if (_fireCooldown > 0) continue;

                Attack();

                // 발사 후 쿨다운 리셋
                _fireCooldown = _fireRate;
            }
        }

        /// <summary>
        /// 타워에서 투사체를 발사하는 로직을 처리합니다.
        /// </summary>
        /// <param name="dir">투사체가 이동할 방향</param>
        /// <param name="targetTr">공격 대상 트랜스폼</param>
        private void Attack()
        {
            AssertHelper.NotEqualsValue(typeof(UnitAttackController), _damage, uint.MaxValue);
            
            SetFacingDirection();
            _currentTarget.StatController.TakeDamage(_damage);
        }

        /// <summary>
        /// 타겟의 위치를 기준으로 유닛의 좌/우 방향을 설정합니다.
        /// </summary>
        /// <param name="targetPosition">타겟의 월드 위치</param>
        /// <summary>
        /// 타겟의 위치를 기준으로 유닛의 좌/우 방향을 설정합니다.
        /// </summary>
        private void SetFacingDirection()
        {
            AssertHelper.NotNull(typeof(UnitAttackController), _currentTarget);
            
            float dirX = _currentTarget.Transform.position.x - transform.position.x;
            float sign = (dirX < 0) ? -1f : 1f;

            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * sign;
            transform.localScale = scale;
        }

        protected override void OnDestroy()
        {
            CancelTokenHelper.DisposeToken(in _cts);

            base.OnDestroy();
        }
    }
}
