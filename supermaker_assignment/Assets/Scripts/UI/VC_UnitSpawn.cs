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
        
        protected override void ValidateReferences()
        {
            AssertHelper.NotNull(typeof(VC_UnitSpawn), _vwUnitSpawn);
        }

        public override void Init(DataManager dataManager)
        {
            _prUnitSpawn.Init(dataManager, _vwUnitSpawn);
        }

        protected override void ReleasePresenter()
        {
            _prUnitSpawn.Dispose();
        }
    }
}
