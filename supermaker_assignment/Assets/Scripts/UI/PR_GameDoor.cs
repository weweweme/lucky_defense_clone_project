using System;
using InGame.System;
using Model;
using UniRx;
using Util;

namespace UI
{
    /// <summary>
    /// 게임 시작과 끝을 나타내는 Presenter 클래스입니다.
    /// </summary>
    public sealed class PR_GameDoor : Presenter
    {
        public override void Init(DataManager dataManager, View view)
        {
            AssertHelper.NotNull(typeof(PR_GameDoor), dataManager);
            
            VW_GameDoor vw = view as VW_GameDoor;
            AssertHelper.NotNull(typeof(PR_GameDoor), vw);
            
            MDL_GameSystem mdlSystem = dataManager.GameSystem;
            AssertHelper.NotNull(typeof(PR_GameDoor), mdlSystem);
            mdlSystem.OnGameFlow
                .Where(state => state == EGameState.GameOver)
                .Subscribe(_ => vw!.EndGameDirection().Forget())
                .AddTo(disposable);
        }
    }
}
