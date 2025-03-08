using System;
using InGame.System;
using Model;
using UniRx;
using Util;

namespace UI
{
    /// <summary>
    /// 다음 웨이브까지 남은 시간을 관리하는 Presenter 클래스입니다. 
    /// </summary>
    public sealed class PR_NextWaveTimer : Presenter
    {
        public override void Init(DataManager dataManager, View view)
        {
            AssertHelper.NotNull(typeof(PR_NextWaveTimer), dataManager);

            VW_NextWaveTimer vw = view as VW_NextWaveTimer;
            AssertHelper.NotNull(typeof(PR_NextWaveTimer), vw);
            
            MDL_Wave mdl = dataManager.Wave;
            AssertHelper.NotNull(typeof(PR_NextWaveTimer), mdl);
            mdl.NextWaveCountDown
                .Subscribe(vw!.SetTime)
                .AddTo(disposable);
        }
    }
}
