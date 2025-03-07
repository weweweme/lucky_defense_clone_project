using System;
using InGame.System;
using Util;

namespace UI
{
    /// <summary>
    /// 신화 유닛 리스트의 Presenter 클래스입니다.
    /// </summary>
    public class PR_MythicUnitList : Presenter
    {
        public override void Init(DataManager dataManager, View view)
        {
            AssertHelper.NotNull(typeof(PR_MythicUnitList), dataManager);
            
            VW_MythicUnitList vw = view as VW_MythicUnitList;
            AssertHelper.NotNull(typeof(PR_MythicUnitList), vw);
        }
    }
}
