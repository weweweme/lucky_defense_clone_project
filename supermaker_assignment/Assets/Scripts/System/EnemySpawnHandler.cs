using Enemy;
using UniRx;
using UnityEngine;
using Util;

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
        
        public EnemySpawnHandler(RootManager rootManager)
        {
            var enemyPathManager = rootManager.EnemyPathNodeManager;
            AssertHelper.NotNull(typeof(EnemySpawnHandler), enemyPathManager);
            
            const int START_IDX = 0;
            AssertHelper.NotNull(typeof(EnemySpawnHandler), enemyPathManager.NorthPathNodes[START_IDX]);
            AssertHelper.NotNull(typeof(EnemySpawnHandler), enemyPathManager.SouthPathNodes[START_IDX]);
            _northSpawnPos = enemyPathManager.NorthPathNodes[START_IDX].position;
            _southSpawnPos = enemyPathManager.SouthPathNodes[START_IDX].position;
            
            _enemyBasePool = rootManager.PoolManager.EnemyBasePool;
            AssertHelper.NotNull(typeof(EnemySpawnHandler), _enemyBasePool);
            
            InitRx(rootManager);
        }
        
        protected override void InitRx(RootManager rootManager)
        {
            rootManager.DataManager.Enemy.OnEnemySpawn
                .Subscribe(SpawnEnemy)
                .AddTo(disposable);
        }
        
        private void SpawnEnemy(SEnemySpawnRequestData data)
        {
            AssertHelper.NotEqualsEnum(typeof(EnemySpawnHandler),data.SpawnMetaData.EnemyType, EEnemyType.None);
            AssertHelper.NotEqualsEnum(typeof(EnemySpawnHandler),data.SpawnSide, EPlayerSide.None);
            
            EnemyRoot enemy = _enemyBasePool.GetObject();
            enemy.transform.position = data.SpawnSide == EPlayerSide.North ? _northSpawnPos : _southSpawnPos;
            enemy.SetUpClassification(data.SpawnMetaData.EnemyType, data.SpawnMetaData.WaveNumber);
            enemy.OnTakeFromPoolInit(data.SpawnSide);
        }
    }
}
