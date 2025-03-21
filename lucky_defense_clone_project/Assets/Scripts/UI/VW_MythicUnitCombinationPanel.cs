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
        [SerializeField] private RequiredUnitSlot[] _requiredUnitSlots;
        [SerializeField] internal Button combineBut;
        [SerializeField] private GameObject _combineButDenyPanel;
        [SerializeField] private TextMeshProUGUI _combineButDenyText;
        
        private MDL_UnitResources _mdlUnitResources;
        private MDL_MythicUnitCombination _mdlMythicUnitCombination;
        private MDL_Unit _mdlUnit;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), _canvas);
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), exitBackgroundPanel);
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), exitButton);
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), _unitName);
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), _unitIcon);
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), _unitFullImage);
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), _requiredUnitSlots);
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), combineBut);
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), _combineButDenyPanel);
            
            const int REQUIRE_SLOT_COUNT = 3;
            AssertHelper.EqualsValue(typeof(VW_MythicUnitCombinationPanel), _requiredUnitSlots.Length, REQUIRE_SLOT_COUNT);
        }

        private void Start()
        {
            var dataManager = RootManager.Ins.DataManager;
            
            _mdlUnitResources = dataManager.UnitResources;
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), _mdlUnitResources);
            
            _mdlMythicUnitCombination = dataManager.MythicUnitCombination;
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), _mdlMythicUnitCombination);
            
            _mdlUnit = dataManager.Unit;
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), _mdlUnit);
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

            // 조건에 맞는 Checker 찾기
            UnitCombinationPossibleChecker checker = null;
            foreach (var flagChecker in _mdlMythicUnitCombination.GetCombinationFlagCheckers())
            {
                if (flagChecker.ResultUnitType != data.UnitType) continue;
                
                checker = flagChecker;
                break;
            }
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), checker);

            // 각 조건 유닛의 아이콘과 상태 패널 세팅
            uint flagCount = 0;
            for (int i = 0; i < _requiredUnitSlots.Length; ++i)
            {
                bool isApproved = checker!.HasRequiredUnit(i);
                SetRequiredUnitSlot(_requiredUnitSlots[i], checker!.GetCondition(i), isApproved);
                
                if (!isApproved) continue;
                ++flagCount;
            }
            
            const uint COMBINE_FLAG_COUNT = 3;
            bool canCombine = flagCount == COMBINE_FLAG_COUNT;
            _combineButDenyPanel.SetActive(!canCombine);

            bool isValidNodeExist = _mdlUnit.HasValidNodes;
            if (canCombine)
            {
                if (isValidNodeExist) return;
                _combineButDenyText.SetText("유효한 공간 부족!");
                _combineButDenyPanel.SetActive(true);
            }
            else
            {
                _combineButDenyText.SetText(isValidNodeExist ? "유닛 불충분!" : "유효한 공간 부족!");    
            }
        }

        /// <summary>
        /// 지정된 슬롯에 조건에 맞는 아이콘과 승인/거부 상태 패널을 설정합니다.
        /// </summary>
        private void SetRequiredUnitSlot(RequiredUnitSlot slot, SUnitCombinationFlagCondition condition, bool isApproved)
        {
            UnitMetaData metaData = _mdlUnitResources.GetResource(condition.Grade, condition.Type);
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationPanel), metaData);

            slot.unitIcon.sprite = metaData.Sprite;
            slot.deniedPanel.SetActive(!isApproved);
        }
    }
}
