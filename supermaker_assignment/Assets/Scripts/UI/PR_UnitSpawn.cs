using System;
using InGame.System;
using Util;

namespace UI
{
    /// <summary>
    /// Unit Spawn UI의 Presenter 클래스입니다.
    /// </summary>
    public sealed class PR_UnitSpawn : Presenter
    {
        public override void Init(DataManager dataManager, View view)
        {
            AssertHelper.NotNull(typeof(PR_UnitSpawn), dataManager);
            
            VW_UnitSpawn vwUnitSpawn = view as VW_UnitSpawn;
            AssertHelper.NotNull(typeof(PR_UnitSpawn), vwUnitSpawn);
        }
    }
}
