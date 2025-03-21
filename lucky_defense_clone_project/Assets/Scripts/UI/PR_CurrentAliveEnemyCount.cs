using System;
using InGame.System;
using Model;
using UniRx;
using Util;

namespace UI
{
    /// <summary>
    /// 현재 남은 적의 수를 보여주는 Presenter 클래스입니다.
    /// </summary>
    public sealed class PR_CurrentAliveEnemyCount : Presenter
    {
        public override void Init(DataManager dataManager, View view, CompositeDisposable disposable)
        {
            AssertHelper.NotNull(typeof(PR_CurrentAliveEnemyCount), dataManager);

            VW_CurrentAliveEnemyCount vw = view as VW_CurrentAliveEnemyCount;
            AssertHelper.NotNull(typeof(PR_CurrentAliveEnemyCount), vw);
            
            MDL_Enemy mdl = dataManager.Enemy;
            AssertHelper.NotNull(typeof(PR_CurrentAliveEnemyCount), mdl);
            mdl.CurrentAliveEnemyCount
                .Subscribe(vw!.UpdateAliveEnemyUI)
                .AddTo(disposable);
        }
    }
}
