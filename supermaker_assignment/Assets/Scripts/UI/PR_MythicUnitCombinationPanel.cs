using System;
using InGame.System;
using Model;
using UniRx;
using Util;

namespace UI
{
    /// <summary>
    /// 신화 조합 패널을 관리하는 Presenter 클래스입니다.
    /// </summary>
    public sealed class PR_MythicUnitCombinationPanel : Presenter
    {
        public override void Init(DataManager dataManager, View view)
        {
            AssertHelper.NotNull(typeof(PR_MythicUnitCombinationPanel), dataManager);
            
            VW_MythicUnitCombinationPanel vw = view as VW_MythicUnitCombinationPanel;
            AssertHelper.NotNull(typeof(PR_MythicUnitCombinationPanel), vw);

            MDL_GameSystem mdlGameSystem = dataManager.GameSystem;
            mdlGameSystem.MythicCombinationPanelVisible
                .Subscribe(vw!.SetCanvasActive)
                .AddTo(disposable);
            
            MDL_MythicUnitCombination mdlMythicUnitCombination = dataManager.MythicUnitCombination;
            mdlMythicUnitCombination.OnMythicUnitCombination
                .Subscribe(vw.SetCurrentUnitData)
                .AddTo(disposable);

            ClosePanel(mdlGameSystem, vw);
        }
        
        private void ClosePanel(MDL_GameSystem mdl, VW_MythicUnitCombinationPanel view)
        {
            view.exitBackgroundPanel.OnClickAsObservable()
                .Subscribe(_ => mdl.SetMythicCombinationPanelVisible(false))
                .AddTo(disposable);
            
            view.exitButton.OnClickAsObservable()
                .Subscribe(_ => mdl.SetMythicCombinationPanelVisible(false))
                .AddTo(disposable);
        }
    }
}
