using System;
using Model;
using UniRx;
using Unit;
using Util;

namespace UI
{
    /// <summary>
    /// Unit Attack Range와 관련된 UI의 비즈니스 로직을 담당하는 Presenter 클래스입니다.
    /// </summary>
    public sealed class PR_UnitPlacementSelection : IDisposable
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private readonly MDL_UnitPlacementField _mdlUnitPlacementField;
        private readonly MDL_Currency _mdlCurrency;
        private readonly MDL_MythicUnitCombination _mdlMythicUnitCombination;
        private readonly MDL_Unit _mdlUnit;
        private readonly MDL_UnitResources _mdlUnitResources;

        public PR_UnitPlacementSelection(MDL_UnitPlacementField mdlUnitPlacementField, VW_UnitPlacementSelection view)
        {
            AssertHelper.NotNull(typeof(PR_UnitPlacementSelection), mdlUnitPlacementField);
            AssertHelper.NotNull(typeof(PR_UnitPlacementSelection), view);

            var dataManager = RootManager.Ins.DataManager;
            _mdlCurrency = dataManager.Currency;
            AssertHelper.NotNull(typeof(PR_UnitPlacementSelection), _mdlCurrency);
            _mdlMythicUnitCombination = dataManager.MythicUnitCombination;
            AssertHelper.NotNull(typeof(PR_UnitPlacementSelection), _mdlMythicUnitCombination);
            _mdlUnit = dataManager.Unit;
            AssertHelper.NotNull(typeof(PR_UnitPlacementSelection), _mdlUnit);
            _mdlUnitResources = dataManager.UnitResources;
            AssertHelper.NotNull(typeof(PR_UnitPlacementSelection), _mdlUnitResources);

            _mdlUnitPlacementField = mdlUnitPlacementField;
            AssertHelper.NotNull(typeof(PR_UnitPlacementSelection), _mdlUnitPlacementField);
            mdlUnitPlacementField.SelectedNode
                .Subscribe(view.ShowUnitPlacementField)
                .AddTo(_disposable);
            
            view.unitSellUI.actionButton.OnClickAsObservable()
                .Subscribe(TrySellUnit)
                .AddTo(_disposable);
            
            view.unitMergeUI.actionButton.OnClickAsObservable()
                .Subscribe(TryMergeUnit)
                .AddTo(_disposable);
        }

        /// <summary>
        /// 해당 노드의 유닛을 판매합니다.
        /// </summary>
        private void TrySellUnit(UniRx.Unit _)
        {
            UnitPlacementNode selectedNode = _mdlUnitPlacementField.GetSelectedNode();
            AssertHelper.NotNull(typeof(PR_UnitPlacementSelection), selectedNode);
            
            EUnitGrade grade = selectedNode.UnitGroup.UnitGrade;
            AssertHelper.NotEqualsEnum(typeof(PR_UnitPlacementSelection), grade, EUnitGrade.None);
            
            EUnitType type = selectedNode.UnitGroup.UnitType;
            AssertHelper.NotEqualsEnum(typeof(PR_UnitPlacementSelection), type, EUnitType.None);
            
            selectedNode.SubUnit();
            
            UnitMetaData metaData = _mdlUnitResources.GetResource(grade, type);
            AssertHelper.NotNull(typeof(PR_UnitPlacementSelection), metaData);
            _mdlCurrency.SubGold(metaData.SellPrice);
            
            const uint SELL_UNIT_COUNT = 1;
            ApplyUnitCount(SELL_UNIT_COUNT);

            // 판매 처리 후 노드가 비어있으면 선택된 노드를 null로 변경
            if (selectedNode.UnitGroup.IsEmpty())
            {
                _mdlUnitPlacementField.SelectNode(null);
            }
            
            foreach (var elem in _mdlMythicUnitCombination.GetCombinationFlagCheckers())
            {
                elem.HandleRemoveUnit(selectedNode);
            }
        }

        /// <summary>
        /// 해당 노드의 유닛을 합성합니다
        /// </summary>
        private void TryMergeUnit(UniRx.Unit _)
        {
            UnitPlacementNode selectedNode = _mdlUnitPlacementField.GetSelectedNode();
            AssertHelper.NotNull(typeof(PR_UnitPlacementSelection), selectedNode);
            
            EUnitGrade grade = selectedNode.UnitGroup.UnitGrade;
            AssertHelper.NotEqualsEnum(typeof(PR_UnitPlacementSelection), grade, EUnitGrade.None);
            AssertHelper.NotEqualsEnum(typeof(PR_UnitPlacementSelection), grade, EUnitGrade.Mythic);

            selectedNode.BeforeMergeClearUnit();
            _mdlUnitPlacementField.SelectNode(null);
            foreach (var elem in _mdlMythicUnitCombination.GetCombinationFlagCheckers())
            {
                elem.HandleRemoveUnit(selectedNode);
            }
            
            EUnitGrade targetGrade = GetNextGrade(grade);
            AssertHelper.NotEqualsEnum(typeof(PR_UnitPlacementSelection), targetGrade, EUnitGrade.None);
            
            EUnitType targetType = GetRandomType();
            
            const uint MERGE_UNIT_COUNT = 2; // -3 + 1 = 2
            ApplyUnitCount(MERGE_UNIT_COUNT);
            
            SUnitSpawnRequestData data = new SUnitSpawnRequestData(targetGrade, targetType, EPlayerSide.South);
            _mdlUnit.SpawnUnit(data);
        }
        
        /// <summary>
        /// 조합을 위한 유닛 수량을 적용하는 메서드입니다.
        /// </summary>
        private void ApplyUnitCount(uint subCount)
        {
            uint currentSpawnCount = _mdlUnit.GetCurrentSpawnCount();
            _mdlUnit.SetCurrentSpawnCount(currentSpawnCount - subCount);
        }

        private EUnitGrade GetNextGrade(EUnitGrade grade) => grade + 1;
        
        private EUnitType GetRandomType()
        {
            return UnityEngine.Random.Range(0f, 1f) < 0.5f ? EUnitType.Melee : EUnitType.Ranged;
        }
        
        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
