using System;
using UniRx;

namespace Model
{
    /// <summary>
    /// 웨이브와 관련된 데이터를 관리하는 모델 클래스.
    /// </summary>
    public class MDL_Wave
    {
        // 현재 몇 웨이브인지 나타내는 Rx
        private const int ROUND_INCREMENT = 1;
        private readonly ReactiveProperty<uint> _currentWaveCount = new ReactiveProperty<uint>(0);
        public IReadOnlyReactiveProperty<uint> CurrentWaveCount => _currentWaveCount;
        public uint GetCurrentWaveCount() => _currentWaveCount.Value;
        public void TriggerNextWave() => _currentWaveCount.Value = _currentWaveCount.Value + ROUND_INCREMENT;
        
        // 현재 웨이브의 상태를 나타내는 Rx
        private readonly ReactiveProperty<EWaveState> _waveState = new ReactiveProperty<EWaveState>(EWaveState.Spawning);
        public IReadOnlyReactiveProperty<EWaveState> WaveState => _waveState;
        public void SetWaveState(EWaveState state) => _waveState.Value = state;
        
        // 다음 웨이브까지의 카운트다운을 나타내는 Rx
        private readonly ReactiveProperty<uint> _nextWaveCountDown = new ReactiveProperty<uint>(0);
        public IReadOnlyReactiveProperty<uint> NextWaveCountDown => _nextWaveCountDown;
        public void SetNextWaveCountDown(uint countDown) => _nextWaveCountDown.Value = countDown;
    }
}
