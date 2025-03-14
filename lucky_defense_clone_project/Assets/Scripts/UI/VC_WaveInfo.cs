using System;
using UnityEngine;
using Util;

namespace UI
{
    /// <summary>
    /// 웨이브와 관련된 정보를 표시하는 UI의 Presenter와 View를 관리하는 클래스입니다.
    /// </summary>
    public sealed class VC_WaveInfo : ViewController
    {
        [SerializeField] private VW_NextWaveTimer _vwNextWaveTimer;
        private readonly PR_NextWaveTimer _prNextWaveTimer = new PR_NextWaveTimer();
        
        [SerializeField] private VW_CurrentWaveCount _vwCurrentWaveCount;
        private readonly PR_CurrentWaveCount _prCurrentWaveCount = new PR_CurrentWaveCount();
        
        [SerializeField] private VW_CurrentAliveEnemyCount vwCurrentAliveEnemyCount;
        private readonly PR_CurrentAliveEnemyCount _prCurrentAliveEnemyCount = new PR_CurrentAliveEnemyCount();
        
        [SerializeField] private VW_InitializeWave _vwInitializeWave;
        private readonly PR_InitializeWave _prInitializeWave = new PR_InitializeWave();
        
        protected override void ValidateReferences()
        {
            AssertHelper.NotNull(typeof(VC_WaveInfo), _vwNextWaveTimer);
            AssertHelper.NotNull(typeof(VC_WaveInfo), _vwCurrentWaveCount);
            AssertHelper.NotNull(typeof(VC_WaveInfo), vwCurrentAliveEnemyCount);
            AssertHelper.NotNull(typeof(VC_WaveInfo), _vwInitializeWave);
        }

        public override void Init(DataManager dataManager)
        {
            _prNextWaveTimer.Init(dataManager, _vwNextWaveTimer, disposable);
            _prCurrentWaveCount.Init(dataManager, _vwCurrentWaveCount, disposable);
            _prCurrentAliveEnemyCount.Init(dataManager, vwCurrentAliveEnemyCount, disposable);
            _prInitializeWave.Init(dataManager, _vwInitializeWave, disposable);
        }
    }
}
