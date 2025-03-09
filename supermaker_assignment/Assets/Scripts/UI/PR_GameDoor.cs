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

            // 게임 종료 시 게임 클리어 여부 판단 후 연출 실행
            mdlSystem.OnGameFlow
                .Where(state => state == EGameState.GameOver || state == EGameState.GameClear)
                .Subscribe(state =>
                {
                    bool isClear = state == EGameState.GameClear;
                    vw!.GameEndDirection(isClear).Forget();
                })
                .AddTo(disposable);

            // 게임 시작 시 게임 시작 연출 실행
            mdlSystem.OnGameFlow
                .Where(state => state == EGameState.Start)
                .Subscribe(_ => vw!.StartGameDirection().Forget())
                .AddTo(disposable);
            
            // 게임 시작 버튼 클릭 시 게임 상태 변경 + 버튼 비활성화
            vw!.startBtn.OnClickAsObservable()
                .Take(1) // 최초 1회만 반응하도록 제한
                .Subscribe(_ =>
                {
                    vw.startBtn.interactable = false; // 버튼 비활성화
                    mdlSystem.ChangeGameFlow(EGameState.Start);
                })
                .AddTo(disposable);
        }
    }
}
