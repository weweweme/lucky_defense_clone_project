using System;
using InGame.System;
using Model;
using Util;
using UniRx;

namespace UI
{
    /// <summary>
    /// 유닛 스폰에 필요한 골드량을 표기하는 Presenter 클래스입니다.
    /// </summary>
    public class PR_SpawnNeededGold : Presenter
    {
        public override void Init(DataManager dataManager, View view, CompositeDisposable disposable)
        {
            VW_Currency vwCurrency = view as VW_Currency;
            AssertHelper.NotNull(typeof(PR_SpawnNeededGold), vwCurrency);
            
            MDL_Unit mdl = dataManager.Unit;
            AssertHelper.NotNull(typeof(PR_SpawnNeededGold), mdl);
            
            mdl.SpawnNeededGold
                .Subscribe(vwCurrency!.UpdateCurrency)
                .AddTo(disposable);
        }
    }
}
