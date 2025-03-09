using System;
using InGame.System;
using Model;
using Util;
using UniRx;

namespace UI
{
    /// <summary>
    /// 현재 몇번째 웨이브인지를 보여주는 Presenter 클래스입니다.
    /// </summary>
    public sealed class PR_CurrentWaveCount : Presenter
    {
        public override void Init(DataManager dataManager, View view, CompositeDisposable disposable)
        {
            AssertHelper.NotNull(typeof(PR_CurrentWaveCount), dataManager);

            VW_CurrentWaveCount vw = view as VW_CurrentWaveCount;
            AssertHelper.NotNull(typeof(PR_CurrentWaveCount), vw);
            
            MDL_Wave mdl = dataManager.Wave;
            AssertHelper.NotNull(typeof(PR_CurrentWaveCount), mdl);
            mdl.CurrentWaveCount
                .Subscribe(vw!.SetWaveCount)
                .AddTo(disposable);
        }
    }
}
