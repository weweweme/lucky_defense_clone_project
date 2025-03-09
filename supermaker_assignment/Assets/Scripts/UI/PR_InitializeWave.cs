using System;
using InGame.System;
using Model;
using UniRx;
using Util;

namespace UI
{
    public sealed class PR_InitializeWave : Presenter
    {
        public override void Init(DataManager dataManager, View view, CompositeDisposable disposable)
        {
            AssertHelper.NotNull(typeof(PR_InitializeWave), dataManager);
            
            VW_InitializeWave vw = view as VW_InitializeWave;
            AssertHelper.NotNull(typeof(PR_InitializeWave), vw);
            
            MDL_GameSystem mdlSystem = dataManager.GameSystem;
            AssertHelper.NotNull(typeof(PR_InitializeWave), mdlSystem);
            
            mdlSystem.OnGameFlow
                .Where(state => state == EGameState.Playing)
                .Subscribe(_ => vw!.StartWaveCountdown().Forget())
                .AddTo(disposable);
        }
    }
}
