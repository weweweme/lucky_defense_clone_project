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
        
        [SerializeField] private VW_Currency _vwSpawnNeededGold;
        private readonly PR_SpawnNeededGold _prSpawnNeededGold = new PR_SpawnNeededGold();
        
        protected override void ValidateReferences()
        {
            AssertHelper.NotNull(typeof(VC_UnitSpawn), _vwUnitSpawn);
            AssertHelper.NotNull(typeof(VC_UnitSpawn), _vwSpawnNeededGold);
        }

        public override void Init(DataManager dataManager)
        {
            _prUnitSpawn.Init(dataManager, _vwUnitSpawn);
            _prSpawnNeededGold.Init(dataManager, _vwSpawnNeededGold);
        }

        protected override void ReleasePresenter()
        {
            _prUnitSpawn.Dispose();
            _prSpawnNeededGold.Dispose();
        }
    }
}
