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
        
        private readonly MDL_Enemy _mdlEnemy;
        internal MDL_Enemy MdlEnemy => _mdlEnemy;

        private readonly EnemyPathManager _pathManager;
        internal EnemyPathManager PathManager => _pathManager;

        public EnemyDependencyContainer(GameManager rootManager)
        {
            _enemyBasePool = rootManager.PoolManager.EnemyBasePool;
            _mdlEnemy = rootManager.DataManager.Enemy;
            _pathManager = rootManager.EnemyPathManager;
        }
    }
}
