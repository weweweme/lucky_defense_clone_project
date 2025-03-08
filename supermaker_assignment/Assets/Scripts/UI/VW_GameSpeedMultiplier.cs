using InGame.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    /// <summary>
    /// 게임 속도를 조절하는 UI의 View 클래스입니다
    /// </summary>
    public sealed class VW_GameSpeedMultiplier : View
    {
        [SerializeField] private TextMeshProUGUI _multiplierTxt;
        [SerializeField] internal Button speedUpBut;
        
        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_GameSpeedMultiplier), _multiplierTxt);
            AssertHelper.NotNull(typeof(VW_GameSpeedMultiplier), speedUpBut);
        }
        
        public void SetMultiple(string multiple) => _multiplierTxt.SetText(multiple);
    }
}
