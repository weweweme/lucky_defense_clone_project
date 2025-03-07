using System;
using InGame.System;
using Model;
using TMPro;
using Unit;
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
        [SerializeField] internal Image unitIcon;
        [SerializeField] internal Image unitFullImage;
        
        private MDL_UnitResources _mdlUnitResources;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), canvas);
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), exitBackgroundPanel);
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), exitButton);
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), unitName);
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), unitIcon);
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), unitFullImage);
        }

        private void Start()
        {
            _mdlUnitResources = RootManager.Ins.DataManager.UnitResources;
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), _mdlUnitResources);
        }
        
        public void SetCanvasActive(bool isActive) => canvas.enabled = isActive;
        public void SetCurrentUnitData(SCurrentMythicUnitCombinationData data)
        {
            AssertHelper.NotEqualsEnum(typeof(VW_MythicUnitCombinationPanel), data.UnitType, EUnitType.None);
            
            UnitMetaData metaData = _mdlUnitResources.GetResource(EUnitGrade.Mythic, data.UnitType);
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), metaData);
            
            unitName.SetText(data.UnitName);
            unitIcon.sprite = metaData.Sprite;
            unitFullImage.sprite = _mdlUnitResources.GetMythicUnitFullSprite(data.UnitType);
        }
    }
}
