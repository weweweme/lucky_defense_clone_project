using InGame.System;
using TMPro;
using UnityEngine;
using Util;

namespace UI
{
    /// <summary>
    /// 게임 속도를 조절하는 UI의 View 클래스입니다
    /// </summary>
    public sealed class VW_GameSpeedMultiplier : View
    {
        [SerializeField] private TextMeshProUGUI _multiplierText;
        
        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_GameSpeedMultiplier), _multiplierText);
        }
        
        public void SetMultiple(string multiple) => _multiplierText.SetText(multiple);
    }
}
