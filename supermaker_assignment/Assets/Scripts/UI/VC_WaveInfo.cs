using System;
using UnityEngine;
using UnityEngine.Serialization;
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
        
        [SerializeField] private VW_CurrentRemainingEnemyCount _vwCurrentRemainingEnemyCount;
        private readonly PR_CurrentRemainingEnemyCount _prCurrentRemainingEnemyCount = new PR_CurrentRemainingEnemyCount();
        
        protected override void ValidateReferences()
        {
            AssertHelper.NotNull(typeof(VC_WaveInfo), _vwNextWaveTimer);
            AssertHelper.NotNull(typeof(VC_WaveInfo), _vwCurrentWaveCount);
            AssertHelper.NotNull(typeof(VC_WaveInfo), _vwCurrentRemainingEnemyCount);
        }

        public override void Init(DataManager dataManager)
        {
            _prNextWaveTimer.Init(dataManager, _vwNextWaveTimer);
            _prCurrentWaveCount.Init(dataManager, _vwCurrentWaveCount);
            _prCurrentRemainingEnemyCount.Init(dataManager, _vwCurrentRemainingEnemyCount);
        }

        protected override void ReleasePresenter()
        {
            _prNextWaveTimer.Dispose();
            _prCurrentWaveCount.Dispose();
            _prCurrentRemainingEnemyCount.Dispose();
        }
    }
}
