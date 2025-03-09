using System;
using InGame.System;
using Model;
using UniRx;
using Util;

namespace UI
{
    /// <summary>
    /// 테스트 패널의 View 클래스입니다.
    /// </summary>
    public sealed class PR_TestConfigPanel : Presenter
    {
        public override void Init(DataManager dataManager, View view)
        {
            AssertHelper.NotNull(typeof(PR_TestConfigPanel), dataManager);
            AssertHelper.NotNull(typeof(PR_TestConfigPanel), view);
            
            VW_TestConfigPanel vw = view as VW_TestConfigPanel;
            AssertHelper.NotNull(typeof(PR_TestConfigPanel), vw);
            
            MDL_GameSystem mdlSystem = dataManager.GameSystem;
            AssertHelper.NotNull(typeof(PR_TestConfigPanel), mdlSystem);
            MDL_Currency mdlCurrency = dataManager.Currency;
            AssertHelper.NotNull(typeof(PR_TestConfigPanel), mdlCurrency);
            
            mdlSystem.OnTestConfigState
                .Subscribe(vw!.SetCanvasActive)
                .AddTo(disposable);

            vw.addGold.selectBut.OnClickAsObservable()
                .Subscribe(_ => mdlCurrency.AddGold(20))
                .AddTo(disposable);
        }
    }
}
