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
        
        protected override void ValidateReferences()
        {
            AssertHelper.NotNull(typeof(VC_WaveInfo), _vwNextWaveTimer);
            AssertHelper.NotNull(typeof(VC_WaveInfo), _vwCurrentWaveCount);
        }

        public override void Init(DataManager dataManager)
        {
            _prNextWaveTimer.Init(dataManager, _vwNextWaveTimer);
            _prCurrentWaveCount.Init(dataManager, _vwCurrentWaveCount);
        }

        protected override void ReleasePresenter()
        {
            _prNextWaveTimer.Dispose();
            _prCurrentWaveCount.Dispose();
        }
    }
}
