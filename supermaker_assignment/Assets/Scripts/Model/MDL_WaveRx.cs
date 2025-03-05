using System;
using UniRx;

namespace Model
{
    public class MDL_WaveRx
    {
        // 현재 몇 웨이브인지 나타내는 Rx
        private const int ROUND_INCREMENT = 1;
        private readonly ReactiveProperty<uint> _currentWave = new ReactiveProperty<uint>(0);
        public IReadOnlyReactiveProperty<uint> CurrentWave => _currentWave;
        public void TriggerNextWave() => _currentWave.Value = _currentWave.Value + ROUND_INCREMENT;
        
        // 현재 웨이브의 상태를 나타내는 Rx
        private readonly ReactiveProperty<EWaveStates> _waveState = new ReactiveProperty<EWaveStates>(EWaveStates.Spawning);
        public IReadOnlyReactiveProperty<EWaveStates> WaveState => _waveState;
        public void SetWaveState(EWaveStates state) => _waveState.Value = state;
        
        // 다음 웨이브까지의 카운트다운을 나타내는 Rx
        private readonly ReactiveProperty<uint> _nextWaveCountDown = new ReactiveProperty<uint>(0);
        public IReadOnlyReactiveProperty<uint> NextWaveCountDown => _nextWaveCountDown;
        public void SetNextWaveCountDown(uint countDown) => _nextWaveCountDown.Value = countDown;
    }
}
