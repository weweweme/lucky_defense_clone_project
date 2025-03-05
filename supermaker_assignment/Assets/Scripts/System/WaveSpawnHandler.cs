using System.Threading;
using Cysharp.Threading.Tasks;
using Model;

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
        private readonly MDL_EnemyRx _mdlEnemyRx;

        public WaveSpawnHandler(GameManager rootManager)
        {
            _mdlEnemyRx = rootManager.DataManager.EnemyRx;
        }
        
        /// <summary>
        /// 현재 웨이브에 따른 적 및 보스 스폰 분기 처리 메서드
        /// </summary>
        /// <param name="currentWave">현재 웨이브 번호</param>
        /// <param name="token">작업 취소 토큰</param>
        public async UniTaskVoid HandleWaveSpawnAsync(uint currentWave, CancellationToken token)
        {
            if (currentWave % 10 == 0)
            {
                await SpawnBossWaveAsync(currentWave, token);
            }
            else
            {
                await SpawnNormalWaveAsync(currentWave, token);
            }
        }

        /// <summary>
        /// 일반 웨이브 스폰 처리 메서드
        /// </summary>
        /// <param name="waveNumber">웨이브 번호</param>
        /// <param name="token">작업 취소 토큰</param>
        private async UniTask SpawnNormalWaveAsync(uint waveNumber, CancellationToken token)
        {
            const float TOTAL_DURATION_SECONDS = 18f;
            const int TOTAL_SPAWN_COUNT = 30;

            float spawnInterval = TOTAL_DURATION_SECONDS / TOTAL_SPAWN_COUNT;

            int spawnedCount = 0;
            float elapsedTime = 0f;

            while (elapsedTime < TOTAL_DURATION_SECONDS)
            {
                if (token.IsCancellationRequested) return;

                // TODO: waveNumber에 따라 에너미가 강해지는 기능 추가
                _mdlEnemyRx.SpawnEnemy(EEnemyType.Default);
                ++spawnedCount;

                if (spawnedCount >= TOTAL_SPAWN_COUNT) break;

                await UniTask.Delay(TimeSpan.FromSeconds(spawnInterval), cancellationToken: token);
                elapsedTime += spawnInterval;
            }
        }

        /// <summary>
        /// 보스 웨이브 스폰 처리 메서드
        /// </summary>
        /// <param name="waveNumber">웨이브 번호</param>
        /// <param name="token">작업 취소 토큰</param>
        private async UniTask SpawnBossWaveAsync(uint waveNumber, CancellationToken token)
        {
            // TODO: waveNumber에 따라 에너미가 강해지는 기능 추가
            // TODO: 보스 소환 이벤트 발행으로 변경
            _mdlEnemyRx.SpawnEnemy(EEnemyType.Default);
        }
    }
}
