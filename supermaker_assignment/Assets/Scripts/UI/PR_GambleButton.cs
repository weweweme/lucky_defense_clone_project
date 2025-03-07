using System;
using InGame.System;
using Model;
using Util;

namespace UI
{
    /// <summary>
    /// 겜블 버튼을 관리하는 Presenter 클래스입니다.
    /// </summary>
    public sealed class PR_GambleButton : Presenter
    {
        public override void Init(DataManager dataManager, View view)
        {
            AssertHelper.NotNull(typeof(PR_GambleButton), dataManager);

            VW_GambleButton vw = view as VW_GambleButton;
            AssertHelper.NotNull(typeof(PR_GambleButton), vw);

            MDL_GameSystem mdl = dataManager.GameSystem;
            AssertHelper.NotNull(typeof(PR_GambleButton), mdl);
        }
    }
}
