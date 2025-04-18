using System.Threading;
using Cysharp.Threading.Tasks;
using Model;
using UniRx;
using Util;

namespace System
{
    /// <summary>
    /// 게임 내 웨이브 진행을 관리하는 클래스입니다.
    /// 웨이브의 시작, 종료, 다음 웨이브 대기 루틴을 비동기 루프로 관리합니다.
    /// </summary>
    public sealed class WaveManager : IDisposable
    {
        /// <summary>
        /// 웨이브 루프의 취소를 위한 CancellationTokenSource입니다.
        /// </summary>
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        
        /// <summary>
        /// 클래스의 Rx를 관리하기 위한 CompositeDisposable입니다.
        /// </summary>
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        /// <summary>
        /// 웨이브 정보를 관리하는 데이터 모델입니다.
        /// </summary>
        private readonly MDL_Wave _mdlWave;
        
        /// <summary>
        /// 게임 시스템 정보를 관리하는 데이터 모델입니다.
        /// </summary>
        private readonly MDL_GameSystem _mdlSystem;
        
        /// <summary>
        /// 에너미와 관련된 데이터를 관리하는 데이터 모델입니다.
        /// </summary>
        private readonly MDL_Enemy _mdlEnemy;

        /// <summary>
        /// 현재 웨이브에서의 몬스터 및 보스 스폰 로직을 관리하는 핸들러입니다.
        /// </summary>
        private readonly WaveSpawnHandler _spawnHandler;

        public WaveManager(RootManager rootManager)
        {
            var dataManager = rootManager.DataManager;
            _mdlWave = dataManager.Wave;
            _mdlSystem = dataManager.GameSystem;
            _mdlEnemy = dataManager.Enemy;
            _spawnHandler = new WaveSpawnHandler(rootManager, _disposable);
        }
        
        /// <summary>
        /// 웨이브 관리 루틴을 시작하는 초기화 메서드입니다.
        /// </summary>
        public void WaveStart()
        {
            LoopAsync(_cts.Token).Forget();
        }

        /// <summary>
        /// 웨이브의 시작, 종료, 대기 과정을 순차적으로 반복하는 비동기 루프입니다.
        /// </summary>
        /// <param name="token">작업 취소 토큰</param>
        private async UniTaskVoid LoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                uint currentWave = _mdlWave.GetCurrentWaveCount();
                const uint FINAL_WAVE = 20;

                if (currentWave == FINAL_WAVE - 1)
                {
                    StartWave(token);
                    bool isSuccess = await FinalWaveCountdown(token);

                    if (isSuccess)
                        GameClear();
                    else
                        GameOver();

                    break;
                }

                StartWave(token);

                await WaitForNextWave(token); // 20번째 웨이브는 여기 생략됨

                EndWave();
            }
        }

        /// <summary>
        /// 게임 클리어 처리 메서드
        /// </summary>
        private void GameClear()
        {
            _mdlSystem.ChangeGameFlow(EGameState.GameClear);
        }
        
        private void GameOver()
        {
            _mdlSystem.ChangeGameFlow(EGameState.GameOver);
        }

        /// <summary>
        /// 웨이브 시작 시 실행되는 비동기 메서드입니다.
        /// </summary>
        /// <param name="token">작업 취소 토큰</param>
        private void StartWave(CancellationToken token)
        {
            _mdlWave.TriggerNextWave();
            _mdlWave.SetWaveState(EWaveState.Spawning);
            
            uint currentWave = _mdlWave.GetCurrentWaveCount();
            
            _spawnHandler.HandleWaveSpawnAsync(currentWave, token).Forget();
            
            _mdlWave.SetWaveState(EWaveState.InProgress);
        }
        
        /// <summary>
        /// 다음 웨이브 시작 전 대기 시간을 처리하는 비동기 메서드입니다.
        /// </summary>
        /// <param name="token">작업 취소 토큰</param>
        private async UniTask WaitForNextWave(CancellationToken token)
        {
            const uint NEXT_ROUND_WAIT_SECONDS = 15;
            const uint COUNTDOWN_INTERVAL_SECONDS = 1;
            
            for (uint i = 0; i < NEXT_ROUND_WAIT_SECONDS; ++i)
            {
                _mdlWave.SetNextWaveCountDown(NEXT_ROUND_WAIT_SECONDS - i);

                await UniTask.Delay(TimeSpan.FromSeconds(COUNTDOWN_INTERVAL_SECONDS), cancellationToken: token);
            }
        }
        
        private async UniTask<bool> FinalWaveCountdown(CancellationToken token)
        {
            const uint FINAL_WAVE_TIME = 60;
            const uint COUNTDOWN_INTERVAL_SECONDS = 1;

            for (uint i = 0; i < FINAL_WAVE_TIME; ++i)
            {
                _mdlWave.SetNextWaveCountDown(FINAL_WAVE_TIME - i);

                if (_mdlEnemy.GetCurrentEnemyCount() == 0)
                    return true; // 모든 적 처치 → 클리어

                await UniTask.Delay(TimeSpan.FromSeconds(COUNTDOWN_INTERVAL_SECONDS), cancellationToken: token);
            }

            return _mdlEnemy.GetCurrentEnemyCount() == 0; // 남은 적이 없으면 클리어, 남아있으면 실패
        }

        /// <summary>
        /// 웨이브 종료 시 실행되는 비동기 메서드입니다.
        /// </summary>
        /// <param name="token">작업 취소 토큰</param>
        private void EndWave()
        {
            _mdlWave.SetWaveState(EWaveState.Waiting);
        }

        /// <summary>
        /// 웨이브 매니저에서 사용하는 모든 리소스를 정리하고 해제합니다.
        /// </summary>
        public void Dispose()
        {
            _disposable.Dispose();
            CancelTokenHelper.DisposeToken(in _cts);
        }
    }
}
