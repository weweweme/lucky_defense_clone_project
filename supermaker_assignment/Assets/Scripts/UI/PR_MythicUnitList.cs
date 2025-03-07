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
        public override void Init(DataManager dataManager, View view)
        {
            AssertHelper.NotNull(typeof(PR_MythicUnitList), dataManager);
            
            VW_MythicUnitList vw = view as VW_MythicUnitList;
            AssertHelper.NotNull(typeof(PR_MythicUnitList), vw);
            
            MDL_MythicUnitCombination mdl = dataManager.MythicUnitCombination;
            AssertHelper.NotNull(typeof(PR_MythicUnitList), mdl);

            foreach (var elem in vw!.mythicUnitItemList)
            {
                elem.unitButton.OnClickAsObservable()
                    .Subscribe(_ => mdl.DisplayMythicUnitCombination(new SCurrentMythicUnitCombinationData(elem.unitName, elem.unitType)))
                    .AddTo(disposable);
            }
        }
    }
}
