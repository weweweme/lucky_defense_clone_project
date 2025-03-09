using System.Threading;
using Cysharp.Threading.Tasks;
using Model;
using UniRx;
using UnityEditor.Rendering;
using UnityEngine;

namespace System
{
    /// <summary>
    /// 웨이브 조건 및 분기 처리를 담당하는 유틸 클래스
    /// </summary>
    public class WaveSpawnHandler
    {
        /// <summary>
        /// 적 유닛의 생성 및 관리 처리를 담당하는 데이터 모델 참조입니다.
        /// </summary>
        private readonly MDL_Enemy _mdlEnemy;
        
        /// <summary>
        /// 게임 시스템 관련 정보를 관리하는 데이터 모델 참조입니다.
        /// </summary>
        private readonly MDL_GameSystem _mdlGameSystem;
        
        /// <summary>
        /// 적 유닛 스폰 메타데이터 입니다.
        /// </summary>
        private readonly EnemySpawnMetaData _currentSpawnMetaData = new EnemySpawnMetaData();
        
        /// <summary>
        /// 현재 맵에 존재하는 적의 수를 나타내는 ReactiveProperty입니다.
        /// </summary>
        private readonly IReadOnlyReactiveProperty<uint> _currentEnemyCount;

        public WaveSpawnHandler(RootManager rootManager, CompositeDisposable disposable)
        {
            _mdlEnemy = rootManager.DataManager.Enemy;
            _mdlGameSystem = rootManager.DataManager.GameSystem;
            _currentEnemyCount = _mdlEnemy.CurrentAliveEnemyCount;
            
            _mdlEnemy.OnOneEnemySpawn
                .Subscribe(_ => SpawnOneEnemy())
                .AddTo(disposable);
        }
        
        /// <summary>
        /// 현재 웨이브에 따른 적 및 보스 스폰 분기 처리 메서드
        /// </summary>
        /// <param name="currentWave">현재 웨이브 번호</param>
        /// <param name="token">작업 취소 토큰</param>
        public async UniTaskVoid HandleWaveSpawnAsync(uint currentWave, CancellationToken token)
        {
            // TestWaveSpawn().Forget();
            
            EEnemyType enemyType = (currentWave % 10 == 0) ? EEnemyType.Boss : EEnemyType.Common;
            _currentSpawnMetaData.SetData(enemyType, currentWave);
            
            if (enemyType == EEnemyType.Boss)
            {
                // 보스 웨이브의 경우 스폰되면서 웨이브 인덱스에 따라 능력치가 세팅됨
                // 따라서 하나의 에너미만 생성
                SpawnOneEnemy();
            }
            else
            {
                await SpawnNormalWaveAsync(token);
            }
        }
        
        public async UniTaskVoid TestWaveSpawn()
        {
            for (int i = 0; i < 100; i++)
            {
                _currentSpawnMetaData.SetData(EEnemyType.Common, (uint)i);
                SpawnEnemy(EPlayerSide.North);
                SpawnEnemy(EPlayerSide.South);
                
                await UniTask.Delay(50); 
            }
        }

        /// <summary>
        /// 일반 웨이브 스폰 처리 메서드
        /// </summary>
        /// <param name="waveNumber">웨이브 번호</param>
        /// <param name="token">작업 취소 토큰</param>
        private async UniTask SpawnNormalWaveAsync(CancellationToken token)
        {
            const float TOTAL_DURATION_SECONDS = 18f;
            const int TOTAL_SPAWN_COUNT = 30;

            float spawnInterval = TOTAL_DURATION_SECONDS / TOTAL_SPAWN_COUNT;

            int spawnedCount = 0;
            float elapsedTime = 0f;

            while (elapsedTime < TOTAL_DURATION_SECONDS)
            {
                if (token.IsCancellationRequested) return;

                SpawnEnemy(EPlayerSide.North);
                SpawnEnemy(EPlayerSide.South);
                
                ++spawnedCount;

                if (spawnedCount >= TOTAL_SPAWN_COUNT) break;

                await UniTask.Delay(TimeSpan.FromSeconds(spawnInterval), cancellationToken: token);
                elapsedTime += spawnInterval;
            }
        }

        /// <summary>
        /// 하나의 유닛만 스폰 처리하는 메서드
        /// </summary>
        /// <param name="waveNumber">웨이브 번호</param>
        /// <param name="token">작업 취소 토큰</param>
        private void SpawnOneEnemy()
        {
            SpawnEnemy(EPlayerSide.North);
            SpawnEnemy(EPlayerSide.South);
        }
        
        /// <summary>
        /// 지정된 진영에 적을 스폰하고 현재 적 수를 갱신하는 메서드입니다.
        /// </summary>
        /// <param name="side">적이 소환될 진영</param>
        private void SpawnEnemy(EPlayerSide side)
        {
            SEnemySpawnRequestData data = new SEnemySpawnRequestData(_currentSpawnMetaData, side);
            _mdlEnemy.SpawnEnemy(data);
            _mdlEnemy.SetCurrentEnemyCount(_currentEnemyCount.Value + 1);
            
            const uint GAME_OVER_ENEMY_COUNT = 100;
            if (_currentEnemyCount.Value != GAME_OVER_ENEMY_COUNT) return;
            
            _mdlGameSystem.ChangeGameFlow(EGameState.GameOver);
        }
    }
}
