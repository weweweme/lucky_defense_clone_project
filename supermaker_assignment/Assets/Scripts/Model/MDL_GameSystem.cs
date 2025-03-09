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
        
        // 현재 게임 속도가 더블 스피드인지 나타내는 Rx.
        private readonly ReactiveProperty<bool> _isDoubleSpeed = new ReactiveProperty<bool>(false);
        public IReactiveProperty<bool> IsDoubleSpeed => _isDoubleSpeed;
        public void SetDoubleGameSpeed(bool value) => _isDoubleSpeed.Value = value;
        public bool IsDoubleSpeedActive() => _isDoubleSpeed.Value;
        
        // 현재 테스트 설정 중인지 나타내는 Rx.
        private readonly BehaviorSubject<ETestConfigState> _onTestConfigState = new BehaviorSubject<ETestConfigState>(ETestConfigState.Close);
        public IObservable<ETestConfigState> OnTestConfigState => _onTestConfigState;
        public void ChangeTestConfigState(ETestConfigState state) => _onTestConfigState.OnNext(state);
        public ETestConfigState GetCurrentTestConfigState() => _onTestConfigState.Value;
    }
}
