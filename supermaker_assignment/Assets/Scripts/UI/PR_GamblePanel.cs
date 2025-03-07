using System;
using InGame.System;
using Model;
using UniRx;
using Util;

namespace UI
{
    /// <summary>
    /// 도박 패널을 관리하는 Presenter 클래스입니다.
    /// </summary>
    public sealed class PR_GamblePanel : Presenter
    {
        public override void Init(DataManager dataManager, View view)
        {
            AssertHelper.NotNull(typeof(PR_GamblePanel), dataManager);

            VW_GamblePanel vw = view as VW_GamblePanel;
            AssertHelper.NotNull(typeof(PR_GamblePanel), vw);
            
            MDL_GameSystem mdl = dataManager.GameSystem;
            AssertHelper.NotNull(typeof(PR_MythicUnitCombinationPanel), mdl);
            vw!.exitBackgroundPanel.OnClickAsObservable()
                .Subscribe(_ => mdl.SetGamblePanelVisible(false))
                .AddTo(disposable);
            vw.exitButton.OnClickAsObservable()
                .Subscribe(_ => mdl.SetGamblePanelVisible(false))
                .AddTo(disposable);
            mdl.GamblePanelVisible
                .Subscribe(vw.SetGamblePanelVisible)
                .AddTo(disposable);
        }
    }
}
