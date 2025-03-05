using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

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
        /// 웨이브 관리 루틴을 시작하는 초기화 메서드입니다.
        /// </summary>
        public void Init()
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
                await StartWave(token);
                await EndWave(token);
                await WaitForNextWave(token);
            }
        }

        /// <summary>
        /// 웨이브 시작 시 실행되는 비동기 메서드입니다.
        /// </summary>
        /// <param name="token">작업 취소 토큰</param>
        private async UniTask StartWave(CancellationToken token)
        {
            // TODO: 웨이브 시작 로직 구현 예정
        }

        /// <summary>
        /// 웨이브 종료 시 실행되는 비동기 메서드입니다.
        /// </summary>
        /// <param name="token">작업 취소 토큰</param>
        private async UniTask EndWave(CancellationToken token)
        {
            // TODO: 웨이브 종료 로직 구현 예정
        }

        /// <summary>
        /// 다음 웨이브 시작 전 대기 시간을 처리하는 비동기 메서드입니다.
        /// </summary>
        /// <param name="token">작업 취소 토큰</param>
        private async UniTask WaitForNextWave(CancellationToken token)
        {
            // TODO: 다음 웨이브 대기 로직 구현 예정
        }

        /// <summary>
        /// 웨이브 매니저에서 사용하는 모든 리소스를 정리하고 해제합니다.
        /// </summary>
        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
