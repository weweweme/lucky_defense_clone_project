using System;
using UniRx;

namespace UI
{
    /// <summary>
    /// Enemy의 체력을 표시하는 UI의 Presenter 클래스입니다.
    /// </summary>
    public sealed class PR_EnemyHP : IDisposable
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public void InitRx(VW_EnemyHP view)
        {
            
        }
        
        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
