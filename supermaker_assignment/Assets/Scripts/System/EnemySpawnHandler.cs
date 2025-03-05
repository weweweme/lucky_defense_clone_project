using Enemy;
using UniRx;
using UnityEngine;

namespace System
{
    /// <summary>
    /// 적 스폰 로직을 담당하는 핸들러 클래스입니다.
    /// 게임 내 적 스폰 이벤트를 감지하고, 해당 이벤트 발생 시 적을 생성하는 역할을 합니다.
    /// </summary>
    public sealed class EnemySpawnHandler : SpawnHandlerBase
    {
        private readonly Vector3 _northSpawnPos;
        private readonly Vector3 _southSpawnPos;
        private readonly EnemyBasePool _enemyBasePool;
        
        public EnemySpawnHandler(GameManager rootManager)
        {
            var enemyPathManager = rootManager.EnemyPathManager;
            const int START_IDX = 0;
            _northSpawnPos = enemyPathManager.NorthPathNodes[START_IDX].position;
            _southSpawnPos = enemyPathManager.SouthPathNodes[START_IDX].position;
            
            _enemyBasePool = rootManager.PoolManager.EnemyBasePool;
            
            InitRx(rootManager);
        }
        
        /// <summary>
        /// GameManager로부터 적 스폰 관련 스트림을 구독하고,
        /// 이벤트 발생 시 적을 생성하는 초기화 로직을 구성합니다.
        /// </summary>
        /// <param name="rootManager">게임 매니저 인스턴스</param>
        protected override void InitRx(GameManager rootManager)
        {
            rootManager.DataManager.EnemyRx.OnEnemySpawn
                .Subscribe(SpawnEnemy)
                .AddTo(disposable);
        }
        
        private void SpawnEnemy(EEnemyType type)
        {
            // TODO: type에 따라 스폰할 에너미의 데이터를 셋업하는 기능 추가
            EnemyRoot enemy = _enemyBasePool.GetObject();
            enemy.transform.position = _northSpawnPos;
            
            enemy = _enemyBasePool.GetObject();
            enemy.transform.position = _southSpawnPos;
        }
    }
}
