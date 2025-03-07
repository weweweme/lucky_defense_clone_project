using System;
using InGame.System;
using TMPro;
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
        [SerializeField] internal TextMeshProUGUI unitName;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), canvas);
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), exitBackgroundPanel);
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), exitButton);
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), unitName);
        }
        
        public void SetCanvasActive(bool isActive) => canvas.enabled = isActive;
        public void SetCurrentUnitData(SCurrentMythicUnitCombinationData data)
        {
            AssertHelper.NotEqualsEnum(typeof(VW_MythicUnitCombinationPanel), data.UnitType, EUnitType.None);
            
            unitName.SetText(data.UnitName);
        }
    }
}
