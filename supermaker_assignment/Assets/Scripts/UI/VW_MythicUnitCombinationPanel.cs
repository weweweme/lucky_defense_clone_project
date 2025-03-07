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
        [SerializeField] internal Button exitBackgroundPanel;
        [SerializeField] internal Button exitButton;

        [SerializeField] private Canvas _canvas;
        [SerializeField] private TextMeshProUGUI _unitName;
        [SerializeField] private Image _unitIcon;
        [SerializeField] private Image _unitFullImage;
        [SerializeField] private Image _requiredFirstUnitIcon;
        [SerializeField] private Image _requiredSecondUnitIcon;
        [SerializeField] private Image _requiredThirdUnitIcon;
        
        private MDL_UnitResources _mdlUnitResources;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), _canvas);
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), exitBackgroundPanel);
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), exitButton);
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), _unitName);
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), _unitIcon);
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), _unitFullImage);
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), _requiredFirstUnitIcon);
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), _requiredSecondUnitIcon);
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), _requiredThirdUnitIcon);
        }

        private void Start()
        {
            _mdlUnitResources = RootManager.Ins.DataManager.UnitResources;
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), _mdlUnitResources);
        }
        
        public void SetCanvasActive(bool isActive) => _canvas.enabled = isActive;
        public void SetCurrentUnitData(SCurrentMythicUnitCombinationData data)
        {
            AssertHelper.NotEqualsEnum(typeof(VW_MythicUnitCombinationPanel), data.UnitType, EUnitType.None);
            
            UnitMetaData metaData = _mdlUnitResources.GetResource(EUnitGrade.Mythic, data.UnitType);
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), metaData);
            
            _unitName.SetText(data.UnitName);
            _unitIcon.sprite = metaData.Sprite;
            _unitFullImage.sprite = _mdlUnitResources.GetMythicUnitFullSprite(data.UnitType);
        }
    }
}
