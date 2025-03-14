using System;
using InGame.System;
using Model;
using UniRx;
using Util;

namespace UI
{
    /// <summary>
    /// 테스트 설정 패널을 열고 닫는 토글의 Presenter 클래스입니다.
    /// </summary>
    public sealed class PR_TestConfigToggle : Presenter
    {
        public override void Init(DataManager dataManager, View view, CompositeDisposable disposable)
        {
            AssertHelper.NotNull(typeof(PR_TestConfigToggle), dataManager);
            
            VW_TestConfigToggle vw = view as VW_TestConfigToggle;
            AssertHelper.NotNull(typeof(PR_TestConfigToggle), vw);
            
            MDL_GameSystem mdlSystem = dataManager.GameSystem;
            AssertHelper.NotNull(typeof(PR_TestConfigToggle), mdlSystem);
            mdlSystem.OnTestConfigState
                .Subscribe(vw!.SetArrowDirection)
                .AddTo(disposable);
            vw.testConfigToggle.OnClickAsObservable()
                .Subscribe(_ => mdlSystem.ChangeTestConfigState(mdlSystem.GetCurrentTestConfigState() == ETestConfigState.Open ? ETestConfigState.Close : ETestConfigState.Open))
                .AddTo(disposable);
        }
    }
}
