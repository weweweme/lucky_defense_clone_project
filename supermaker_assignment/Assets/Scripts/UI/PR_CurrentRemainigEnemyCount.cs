using System;
using InGame.System;
using Model;
using Util;

namespace UI
{
    /// <summary>
    /// 현재 남은 적의 수를 보여주는 Presenter 클래스입니다.
    /// </summary>
    public sealed class PR_CurrentRemainigEnemyCount : Presenter
    {
        public override void Init(DataManager dataManager, View view)
        {
            AssertHelper.NotNull(typeof(PR_CurrentRemainigEnemyCount), dataManager);

            VW_CurrentRemainigEnemyCount vw = view as VW_CurrentRemainigEnemyCount;
            AssertHelper.NotNull(typeof(PR_CurrentRemainigEnemyCount), vw);
            
            MDL_Wave mdl = dataManager.Wave;
            AssertHelper.NotNull(typeof(PR_CurrentRemainigEnemyCount), mdl);
        }
    }
}
