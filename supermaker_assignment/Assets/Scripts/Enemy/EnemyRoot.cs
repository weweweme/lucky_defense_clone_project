using System;
using UnityEngine;
using Util;

namespace Enemy
{
    /// <summary>
    /// Enemy의 최상위 루트 클래스입니다.
    /// 각 모듈의 참조를 관리하고, 초기화 과정을 수행합니다.
    /// </summary>
    public sealed class EnemyRoot : PooledEntityRootBase
    {
        [SerializeField] internal EnemyMoveController moveController;
        [SerializeField] internal EnemyStatController statController;
        internal EnemyDependencyContainer dependencyContainer;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(EnemyRoot), moveController);
            AssertHelper.NotNull(typeof(EnemyRoot), statController);
        }
        
        public void CreatePooledItemInit(EnemyDependencyContainer container)
        {
            dependencyContainer = container;
            statController.CreatePooledItemInit(this);
        }

        public override void OnTakeFromPoolInit(EPlayerSide side)
        {
            AssertHelper.NotEqualsEnum(typeof(EnemyRoot), side, EPlayerSide.None);

            var enemyPathManager = dependencyContainer.PathNodeManager;
            Transform[] pathNodePtr = side == EPlayerSide.North ? enemyPathManager.NorthPathNodes : enemyPathManager.SouthPathNodes;
            AssertHelper.NotNull(typeof(EnemyRoot), pathNodePtr);
            
            moveController.OnActivate(pathNodePtr);
            statController.OnTakeFromPoolInit();
        }
    }
}
