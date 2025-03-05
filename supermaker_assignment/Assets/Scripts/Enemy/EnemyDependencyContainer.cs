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
        
        private readonly MDL_EnemyRx _mdlEnemy;
        internal MDL_EnemyRx MDLEnemy => _mdlEnemy;

        public EnemyDependencyContainer(EnemyBasePool enemyBasePool, MDL_EnemyRx mdlEnemy)
        {
            _enemyBasePool = enemyBasePool;
            _mdlEnemy = mdlEnemy;
        }
    }
}
