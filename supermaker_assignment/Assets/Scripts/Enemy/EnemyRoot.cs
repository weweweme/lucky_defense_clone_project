using UnityEngine;
using Util;

namespace Enemy
{
    /// <summary>
    /// Enemy의 최상위 루트 클래스입니다.
    /// 각 모듈의 참조를 관리하고, 초기화 과정을 수행합니다.
    /// </summary>
    public sealed class EnemyRoot : MonoBehaviourBase
    {
        [SerializeField] internal EnemyMoveController moveController;
        internal EnemyDependencyContainer dependencyContainer;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(EnemyRoot), moveController);
        }
        
        public void Init(EnemyDependencyContainer container)
        {
            dependencyContainer = container;
        }
    }
}
