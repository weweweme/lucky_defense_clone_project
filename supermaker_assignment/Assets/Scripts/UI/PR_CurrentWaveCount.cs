using System;
using InGame.System;
using Util;

namespace UI
{
    /// <summary>
    /// 현재 몇번째 웨이브인지를 보여주는 Presenter 클래스입니다.
    /// </summary>
    public sealed class PR_CurrentWaveCount : Presenter
    {
        public override void Init(DataManager dataManager, View view)
        {
            AssertHelper.NotNull(typeof(PR_CurrentWaveCount), dataManager);

            VW_CurrentWaveCount vw = view as VW_CurrentWaveCount;
            AssertHelper.NotNull(typeof(PR_CurrentWaveCount), vw);
        }
    }
}
