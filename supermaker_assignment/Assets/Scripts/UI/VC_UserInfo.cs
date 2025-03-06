using System;
using UnityEngine;
using Util;

namespace UI
{
    /// <summary>
    /// UserInfo와 관련된 Presenter와 View를 관리하는 클래스입니다.
    /// </summary>
    public class VC_UserInfo : ViewController
    {
        [SerializeField] private VW_Currency _vwGoldCurrency;
        private readonly PR_GoldCurrency _prGoldCurrency = new PR_GoldCurrency();
        
        protected override void ValidateReferences()
        {
            AssertHelper.NotNull(typeof(VC_UnitSpawn), _vwGoldCurrency);
        }

        public override void Init(DataManager dataManager)
        {
            _prGoldCurrency.Init(dataManager, _vwGoldCurrency);
        }

        protected override void ReleasePresenter()
        {
            _prGoldCurrency.Dispose();
        }
    }
}
