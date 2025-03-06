using System;
using Model;
using UniRx;
using Util;

namespace UI
{
    /// <summary>
    /// Enemy의 체력을 표시하는 UI의 Presenter 클래스입니다.
    /// </summary>
    public sealed class PR_EnemyHP : IDisposable
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private readonly MDL_EnemyStat _mdl;
        private readonly VW_EnemyHP _view;

        public PR_EnemyHP(MDL_EnemyStat mdl, VW_EnemyHP view)
        {
            AssertHelper.NotNull(typeof(PR_EnemyHP), mdl);
            AssertHelper.NotNull(typeof(PR_EnemyHP), view);
            
            _mdl = mdl;
            _view = view;
            
            mdl.Hp
                .Skip(Observer.INITIAL_SUBSCRIPTION_SKIP_COUNT)
                .Subscribe(NormalizeHP)
                .AddTo(_disposable);
        }

        private void NormalizeHP(uint currentHp)
        {
            float maxHp = _mdl.MaxHp;
            float normalizedHp = currentHp / maxHp;
            _view.UpdateHealthUI(normalizedHp);
        }
        
        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
