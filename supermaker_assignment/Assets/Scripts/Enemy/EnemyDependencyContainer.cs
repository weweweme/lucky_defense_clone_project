using System;
using Model;

namespace Enemy
{
    /// <summary>
    /// Enemy에서 사용될 외부 모듈의 참조를 관리하는 컨테이너 클래스입니다.
    /// </summary>
    public sealed class EnemyDependencyContainer : DependencyContainerBase
    {
        internal readonly EnemyBasePool enemyBasePool;
        internal readonly MDL_Enemy mdlEnemy;
        internal readonly MDL_Currency mdlCurrency;
        internal readonly EnemyPathNodeManager pathNodeManager;

        public EnemyDependencyContainer(RootManager rootManager)
        {
            enemyBasePool = rootManager.PoolManager.EnemyBasePool;
            mdlEnemy = rootManager.DataManager.Enemy;
            mdlCurrency = rootManager.DataManager.Currency;
            pathNodeManager = rootManager.EnemyPathNodeManager;
        }
    }
}
