using System;
using InGame.System;
using Model;
using UniRx;
using Util;

namespace UI
{
    /// <summary>
    /// 다이아몬드와 관련된 UI를 관리하는 Presenter 클래스입니다.
    /// </summary>
    public sealed class PR_DiamondCurrency : Presenter
    {
        public override void Init(DataManager dataManager, View view)
        {
            AssertHelper.NotNull(typeof(PR_DiamondCurrency), dataManager);

            MDL_Currency mdl = dataManager.Currency;
            AssertHelper.NotNull(typeof(PR_DiamondCurrency), mdl);
            
            VW_Currency vw = view as VW_Currency;
            AssertHelper.NotNull(typeof(PR_DiamondCurrency), vw);
            
            mdl.Diamond
                .Subscribe(vw!.UpdateCurrency)
                .AddTo(disposable);
        }
    }
}
