using System;
using UniRx;

namespace Model
{
    /// <summary>
    /// 게임 전반적인 이벤트의 Rx를 관리하는 모델 클래스.
    /// </summary>
    public class MDL_GameSystem
    {
        // 게임 플로우와 관련된 Rx.
        private readonly Subject<EGameState> _onGameFlow = new Subject<EGameState>();
        public IObservable<EGameState> OnGameFlow => _onGameFlow;
        public void ChangeGameFlow(EGameState state) => _onGameFlow.OnNext(state);
    }
}
