using System;
using InGame.System;
using Model;
using UniRx;
using Util;

namespace UI
{
    /// <summary>
    /// 신화 유닛 리스트의 Presenter 클래스입니다.
    /// </summary>
    public sealed class PR_MythicUnitList : Presenter
    {
        private MDL_MythicUnitCombination _mdlMythicUnitCombination;
        private MythicUnitListItem _firstMythicUnitListItem;
        private VW_MythicUnitList _vw;
        
        public override void Init(DataManager dataManager, View view, CompositeDisposable disposable)
        {
            AssertHelper.NotNull(typeof(PR_MythicUnitList), dataManager);
            
            _vw = view as VW_MythicUnitList;
            AssertHelper.NotNull(typeof(PR_MythicUnitList), _vw);
            
            MDL_GameSystem mdlSystem = dataManager.GameSystem;
            AssertHelper.NotNull(typeof(PR_MythicUnitList), mdlSystem);
            mdlSystem.MythicCombinationPanelVisible
                .Subscribe(DisplayMythicUnitCombinationPanel)
                .AddTo(disposable);

            _mdlMythicUnitCombination = dataManager.MythicUnitCombination;
            AssertHelper.NotNull(typeof(PR_MythicUnitList), _mdlMythicUnitCombination);
            _firstMythicUnitListItem = _vw!.mythicUnitItemList[0];
            foreach (var elem in _vw!.mythicUnitItemList)
            {
                elem.unitButton.OnClickAsObservable()
                    .Subscribe(_ =>
                    {
                        _vw.highlight.transform.position = elem.transform.position;
                        _mdlMythicUnitCombination.DisplayMythicUnitCombination(new SCurrentMythicUnitCombinationData(elem.unitName, elem.unitType));
                    })
                    .AddTo(disposable);
            }
        }
        
        private void DisplayMythicUnitCombinationPanel(bool value)
        {
            if (!value) return;
            
            _vw.highlight.transform.position = _firstMythicUnitListItem.transform.position;
            _mdlMythicUnitCombination.DisplayMythicUnitCombination(new SCurrentMythicUnitCombinationData(_firstMythicUnitListItem.unitName, _firstMythicUnitListItem.unitType));
        }
    }
}
