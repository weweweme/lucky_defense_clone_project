using System;
using InGame.System;
using Util;

namespace UI
{
    /// <summary>
    /// 도박 패널을 관리하는 Presenter 클래스입니다.
    /// </summary>
    public sealed class PR_GamblePanel : Presenter
    {
        public override void Init(DataManager dataManager, View view)
        {
            AssertHelper.NotNull(typeof(PR_GamblePanel), dataManager);

            VW_GamblePanel vw = view as VW_GamblePanel;
            AssertHelper.NotNull(typeof(PR_GamblePanel), vw);
        }
    }
}
