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
        private MDL_MythicUnitCombination _mdlMythicUnitCombination;

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
            
            _mdlMythicUnitCombination = RootManager.Ins.DataManager.MythicUnitCombination;
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), _mdlMythicUnitCombination);
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

            // 조건에 맞는 CombinationFlagChecker 찾기
            UnitCombinationFlagChecker checker = null;
            foreach (var flagChecker in _mdlMythicUnitCombination.GetCombinationFlagCheckers())
            {
                if (flagChecker.ResultUnitType != data.UnitType) continue;
                
                checker = flagChecker;
                break;
            }
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), checker);

            // 각 조건 유닛의 아이콘 세팅
            SetRequiredUnitIcon(_requiredFirstUnitIcon, checker.GetCondition(0));
            SetRequiredUnitIcon(_requiredSecondUnitIcon, checker.GetCondition(1));
            SetRequiredUnitIcon(_requiredThirdUnitIcon, checker.GetCondition(2));
        }

        /// <summary>
        /// 지정된 조건에 해당하는 유닛의 아이콘을 설정합니다.
        /// </summary>
        private void SetRequiredUnitIcon(Image targetIcon, SUnitCombinationFlagCondition condition)
        {
            UnitMetaData metaData = _mdlUnitResources.GetResource(condition.Grade, condition.Type);
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), metaData);

            targetIcon.sprite = metaData.Sprite;
        }
    }
}
