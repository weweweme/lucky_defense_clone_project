using System.Threading;
using Cysharp.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 웨이브 조건 및 분기 처리를 담당하는 유틸 클래스
    /// </summary>
    public class WaveSpawnHandler
    {
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
            // 여기에 일반 웨이브 몬스터 스폰 로직 추가 예정
            await UniTask.CompletedTask;
        }

        /// <summary>
        /// 보스 웨이브 스폰 처리 메서드
        /// </summary>
        /// <param name="waveNumber">웨이브 번호</param>
        /// <param name="token">작업 취소 토큰</param>
        private async UniTask SpawnBossWaveAsync(uint waveNumber, CancellationToken token)
        {
            // 여기에 보스 스폰 로직 추가 예정
            await UniTask.CompletedTask;
        }
    }
}
