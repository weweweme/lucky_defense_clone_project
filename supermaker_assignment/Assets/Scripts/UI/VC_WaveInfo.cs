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
        
        protected override void ValidateReferences()
        {
            AssertHelper.NotNull(typeof(VC_WaveInfo), _vwNextWaveTimer);
        }

        public override void Init(DataManager dataManager)
        {
            _prNextWaveTimer.Init(dataManager, _vwNextWaveTimer);
        }

        protected override void ReleasePresenter()
        {
            _prNextWaveTimer.Dispose();
        }
    }
}
