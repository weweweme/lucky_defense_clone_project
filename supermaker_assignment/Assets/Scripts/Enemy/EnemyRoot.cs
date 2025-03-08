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
        /// <summary>
        /// 현재 몇번째 웨이브에 생성되었는지 저장하는 변수입니다. 
        /// </summary>
        internal uint currentSpawnWaveIdx;
        internal EEnemyType type;
        
        [SerializeField] internal EnemyMoveController moveController;
        [SerializeField] internal EnemyStatController statController;
        [SerializeField] internal EnemySpriteController spriteController;
        internal EnemyDependencyContainer dependencyContainer;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(EnemyRoot), moveController);
            AssertHelper.NotNull(typeof(EnemyRoot), statController);
        }
        
        public void SetUpClassification(EEnemyType enemyType, uint waveIdx)
        {
            AssertHelper.NotEqualsEnum(typeof(EnemyRoot), enemyType, EEnemyType.None);
            
            type = enemyType;
            currentSpawnWaveIdx = waveIdx;
        }
        
        public override void CreatePooledItemInit(DependencyContainerBase containerBase)
        {
            EnemyDependencyContainer container = containerBase as EnemyDependencyContainer;
            AssertHelper.NotNull(typeof(EnemyRoot), container);
            
            dependencyContainer = container;
            statController.CreatePooledItemInit(this);
            spriteController.ChangeVisible();
        }

        public override void OnTakeFromPoolInit(EPlayerSide side)
        {
            AssertHelper.NotEqualsEnum(typeof(EnemyRoot), side, EPlayerSide.None);

            var enemyPathManager = dependencyContainer.pathNodeManager;
            Transform[] pathNodePtr = side == EPlayerSide.North ? enemyPathManager.NorthPathNodes : enemyPathManager.SouthPathNodes;
            AssertHelper.NotNull(typeof(EnemyRoot), pathNodePtr);
            
            moveController.OnActivate(pathNodePtr);
            statController.OnTakeFromPoolInit();
        }
    }
}
