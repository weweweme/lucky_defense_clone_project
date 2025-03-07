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
        
        // 신화 조합 패널의 on/off 여부를 나타내는 Rx.
        private readonly ReactiveProperty<bool> _mythicCombinationPanelVisible = new ReactiveProperty<bool>(false);
        public IReactiveProperty<bool> MythicCombinationPanelVisible => _mythicCombinationPanelVisible;
        public void SetMythicCombinationPanelVisible(bool value) => _mythicCombinationPanelVisible.Value = value;
        
        // 도박 패널의 on/off 여부를 나타내는 Rx.
        private readonly ReactiveProperty<bool> _gamblePanelVisible = new ReactiveProperty<bool>(false);
        public IReactiveProperty<bool> GamblePanelVisible => _gamblePanelVisible;
        public void SetGamblePanelVisible(bool value) => _gamblePanelVisible.Value = value;
    }
}
