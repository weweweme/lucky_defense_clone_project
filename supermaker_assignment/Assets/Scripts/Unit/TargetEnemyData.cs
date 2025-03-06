using UnityEngine;
using Util;

namespace Unit
{
    /// <summary>
    /// 타겟 정보를 저장하고 관리하는 클래스입니다.
    /// Transform과 EnemyStatController 클래스 참조를 포함하며,
    /// 타겟이 유효한지 또는 사망했는지를 확인할 수 있습니다.
    /// </summary>
    public class TargetEnemyData
    {
        private Transform _transform;
        private EnemyStatController _statController;
        private bool _hasTarget;

        /// <summary>
        /// 타겟의 Transform을 가져옵니다.
        /// </summary>
        public Transform Transform => _transform;

        /// <summary>
        /// 타겟의 IDamageable 인터페이스를 가져옵니다.
        /// </summary>
        public EnemyStatController StatController => _statController;

        /// <summary>
        /// 현재 유효한 타겟이 설정되어 있는지 여부를 반환합니다.
        /// </summary>
        public bool HasTarget => _hasTarget;

        /// <summary>
        /// 지정된 GameObject에서 Transform과 IDamageable을 추출하여 설정합니다.
        /// </summary>
        /// <param name="targetObject">설정할 대상 GameObject</param>
        public void SetTarget(GameObject targetObject)
        {
            _transform = targetObject.transform;
            _statController = targetObject.GetComponentOrAssert<EnemyStatController>();

            _hasTarget = true;
        }

        /// <summary>
        /// 현재 설정된 타겟 정보를 모두 비웁니다.
        /// </summary>
        public void Clear()
        {
            _transform = null;
            _statController = null;
            _hasTarget = false;
        }

        /// <summary>
        /// 현재 타겟이 사망했는지 확인합니다.
        /// </summary>
        /// <returns>true면 사망, false면 생존</returns>
        public bool IsDead()
        {
            if (!_hasTarget || _statController == null)
                return true;

            return !_statController.CanTakeDamage();
        }
    }
}
