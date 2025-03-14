using InGame.System;
using TMPro;
using UnityEngine;
using Util;

namespace UI
{
    /// <summary>
    /// 현재 몇번째 웨이브인지를 보여주는 View 클래스입니다.
    /// </summary>
    public sealed class VW_CurrentWaveCount : View
    {
        [SerializeField] private TextMeshProUGUI currentWaveCountText;
        
        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_CurrentWaveCount), currentWaveCountText);
        }
        
        public void SetWaveCount(uint waveCount) => currentWaveCountText.SetText($"WAVE {waveCount}");
    }
}
