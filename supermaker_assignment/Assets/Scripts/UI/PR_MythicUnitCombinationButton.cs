using System;
using InGame.System;
using Model;
using UniRx;
using Util;

namespace UI
{
    /// <summary>
    /// 신화 조합 패널을 여는 버튼의 Presenter 클래스입니다.
    /// </summary>
    public sealed class PR_MythicUnitCombinationButton : Presenter
    {
        public override void Init(DataManager dataManager, View view)
        {
            AssertHelper.NotNull(typeof(PR_MythicUnitCombinationButton), dataManager);
            
            VW_MythicUnitCombinationButton vw = view as VW_MythicUnitCombinationButton;
            AssertHelper.NotNull(typeof(PR_MythicUnitCombinationButton), vw);

            MDL_GameSystem mdl = dataManager.GameSystem;
            AssertHelper.NotNull(typeof(PR_MythicUnitCombinationButton), mdl);
            vw!.openPanelBut.OnClickAsObservable()
                .Where(_ => !mdl.MythicCombinationPanelVisible.Value)
                .Subscribe(_ => mdl.SetMythicCombinationPanelVisible(true))
                .AddTo(disposable);
        }
    }
}
