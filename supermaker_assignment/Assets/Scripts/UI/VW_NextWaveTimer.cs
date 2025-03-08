using InGame.System;
using TMPro;
using UnityEngine;
using Util;

namespace UI
{
    /// <summary>
    /// 다음 웨이브까지 남은 시간을 관리하는 View 클래스입니다.
    /// </summary>
    public sealed class VW_NextWaveTimer : View
    {
        [SerializeField] private TextMeshProUGUI nextWaveTimerText;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_NextWaveTimer), nextWaveTimerText);
        }

        public void SetTime(uint seconds) => nextWaveTimerText.SetText($"00 : {seconds:D2}");
    }
}
