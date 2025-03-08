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
        
        [SerializeField] private VW_Currency _vwDiamondCurrency;
        private readonly PR_DiamondCurrency _prDiamondCurrency = new PR_DiamondCurrency();
        
        [SerializeField] private VW_UserSpawnInfo _vwUserSpawnInfo;
        private readonly PR_UserSpawnInfo _prUserSpawnInfo = new PR_UserSpawnInfo();
        
        protected override void ValidateReferences()
        {
            AssertHelper.NotNull(typeof(VC_UnitSpawn), _vwGoldCurrency);
            AssertHelper.NotNull(typeof(VC_UnitSpawn), _vwDiamondCurrency);
            AssertHelper.NotNull(typeof(VC_UnitSpawn), _vwUserSpawnInfo);
        }

        public override void Init(DataManager dataManager)
        {
            _prGoldCurrency.Init(dataManager, _vwGoldCurrency);
            _prDiamondCurrency.Init(dataManager, _vwDiamondCurrency);
            _prUserSpawnInfo.Init(dataManager, _vwUserSpawnInfo);
        }

        protected override void ReleasePresenter()
        {
            _prGoldCurrency.Dispose();
            _prDiamondCurrency.Dispose();
            _prUserSpawnInfo.Dispose();
        }
    }
}
