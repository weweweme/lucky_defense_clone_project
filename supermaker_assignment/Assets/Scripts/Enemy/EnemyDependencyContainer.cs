using System;
using Model;

namespace Enemy
{
    /// <summary>
    /// Enemy에서 사용될 외부 모듈의 참조를 관리하는 컨테이너 클래스입니다.
    /// </summary>
    public class EnemyDependencyContainer
    {
        private readonly EnemyBasePool _enemyBasePool;
        internal EnemyBasePool EnemyBasePool => _enemyBasePool;
        
        private readonly MDL_EnemyRx _mdlEnemyRx;
        internal MDL_EnemyRx MdlEnemyRx => _mdlEnemyRx;

        private readonly EnemyPathManager _pathManager;
        internal EnemyPathManager PathManager;

        public EnemyDependencyContainer(GameManager rootManager)
        {
            _enemyBasePool = rootManager.PoolManager.EnemyBasePool;
            _mdlEnemyRx = rootManager.DataManager.EnemyRx;
            _pathManager = rootManager.EnemyPathManager;
        }
    }
}
