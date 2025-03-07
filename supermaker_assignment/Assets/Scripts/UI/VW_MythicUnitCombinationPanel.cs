using InGame.System;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    /// <summary>
    /// 신화 조합 패널의 View 클래스입니다.
    /// </summary>
    public sealed class VW_MythicUnitCombinationPanel : View
    {
        [SerializeField] internal Canvas canvas;
        [SerializeField] internal Button exitBackgroundPanel;
        [SerializeField] internal Button exitButton;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), canvas);
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), exitBackgroundPanel);
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), exitButton);
        }
        
        public void SetCanvasActive(bool isActive) => canvas.enabled = isActive;
    }
}
