using System;
using InGame.System;
using Model;
using Util;

namespace UI
{
    /// <summary>
    /// User Spawn Info와 관련된 UI의 비즈니스 로직을 담당하는 Presenter 클래스입니다.
    /// </summary>
    public class PR_UserSpawnInfo : Presenter
    {
        public override void Init(DataManager dataManager, View view)
        {
            AssertHelper.NotNull(typeof(PR_UnitSpawn), dataManager);
            
            MDL_Unit mdl = dataManager.Unit;
            AssertHelper.NotNull(typeof(PR_UnitSpawn), mdl);
            
            VW_UserSpawnInfo vw = view as VW_UserSpawnInfo;
            AssertHelper.NotNull(typeof(PR_UnitSpawn), vw);
        }
    }
}
