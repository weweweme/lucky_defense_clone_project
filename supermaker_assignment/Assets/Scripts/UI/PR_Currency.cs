using System;
using InGame.System;
using Model;
using Util;
using UniRx;

namespace UI
{
    /// <summary>
    /// 재화와 관련된 UI를 관리하는 Presenter 클래스입니다.
    /// </summary>
    public class PR_Currency : Presenter
    {
        public override void Init(DataManager dataManager, View view)
        {
            AssertHelper.NotNull(typeof(PR_Currency), dataManager);

            MDL_Currency mdl = dataManager.Currency;
            AssertHelper.NotNull(typeof(PR_Currency), mdl);
            
            VW_Currency vw = view as VW_Currency;
            AssertHelper.NotNull(typeof(PR_UnitSpawn), vw);
            
            mdl.Gold
                .Subscribe(vw!.UpdateCurrency)
                .AddTo(disposable);
        }
    }
}
