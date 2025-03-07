using InGame.System;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    /// <summary>
    /// 도박 패널을 관리하는 View 클래스입니다.
    /// </summary>
    public sealed class VW_GamblePanel : View
    {
        [SerializeField] internal Button exitBackgroundPanel;
        [SerializeField] internal Button exitButton;
        [SerializeField] internal GambleTryItem[] gambleTryItems;
        
        [SerializeField] private Canvas _gambleCanvas;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_GamblePanel), exitBackgroundPanel);
            AssertHelper.NotNull(typeof(VW_GamblePanel), exitButton);
            AssertHelper.NotNull(typeof(VW_GamblePanel), _gambleCanvas);
            
            const int TRY_SLOT_COUNT = 3;
            AssertHelper.EqualsValue(typeof(VW_MythicUnitCombinationPanel), gambleTryItems.Length, TRY_SLOT_COUNT);
        }
        
        public void SetGamblePanelVisible(bool value) => _gambleCanvas.enabled = value;
    }
}
