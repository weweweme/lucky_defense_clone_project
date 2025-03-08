using System;
using InGame.System;
using Model;
using Util;

namespace UI
{
    /// <summary>
    /// 현재 남은 적의 수를 보여주는 Presenter 클래스입니다.
    /// </summary>
    public sealed class PR_CurrentRemainingEnemyCount : Presenter
    {
        public override void Init(DataManager dataManager, View view)
        {
            AssertHelper.NotNull(typeof(PR_CurrentRemainingEnemyCount), dataManager);

            VW_CurrentRemainingEnemyCount vw = view as VW_CurrentRemainingEnemyCount;
            AssertHelper.NotNull(typeof(PR_CurrentRemainingEnemyCount), vw);
            
            MDL_Wave mdl = dataManager.Wave;
            AssertHelper.NotNull(typeof(PR_CurrentRemainingEnemyCount), mdl);
        }
    }
}
