using System;
using UnityEngine;
using Util;

namespace UI
{
    /// <summary>
    /// UnitSpawn과 관련된 Presenter와 View를 관리하는 클래스입니다.
    /// </summary>
    public sealed class VC_UnitSpawn : ViewController
    {
        [SerializeField] private VW_UnitSpawn _vwUnitSpawn;
        private readonly PR_UnitSpawn _prUnitSpawn = new PR_UnitSpawn();
        
        [SerializeField] private VW_Currency _vwCurrency;
        private readonly PR_Currency _prCurrency = new PR_Currency();
        
        protected override void ValidateReferences()
        {
            AssertHelper.NotNull(typeof(VC_UnitSpawn), _vwUnitSpawn);
            AssertHelper.NotNull(typeof(VC_UnitSpawn), _vwCurrency);
        }

        public override void Init(DataManager dataManager)
        {
            _prUnitSpawn.Init(dataManager, _vwUnitSpawn);
            _prCurrency.Init(dataManager, _vwCurrency);
        }

        protected override void ReleasePresenter()
        {
            _prUnitSpawn.Dispose();
            _prCurrency.Dispose();
        }
    }
}
