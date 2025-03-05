using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

namespace System
{
    public sealed class WaveManager : IDisposable
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        public void Init()
        {
            LoopAsync(_cts.Token).Forget();
        }

        private async UniTaskVoid LoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await StartWave(token);
                await EndWave(token);
                await WaitForNextWave(token);
            }
        }

        private async UniTask StartWave(CancellationToken token)
        {
            
        }

        private async UniTask EndWave(CancellationToken token)
        {
            
        }

        private async UniTask WaitForNextWave(CancellationToken token)
        {
            
        }
        
        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
