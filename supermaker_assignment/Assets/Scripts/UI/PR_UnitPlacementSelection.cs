using System;
using Model;
using UniRx;
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

            _mdlUnitPlacementField = mdlUnitPlacementField;
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
            
            selectedNode.SellUnit();
            // TODO: 추후 Grade와 Type에 따라 return하도록 변경.
            _mdlCurrency.SubGold(1);

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

            selectedNode.MergeUnit();
            _mdlUnitPlacementField.SelectNode(null);
            
            EUnitGrade targetGrade = GetNextGrade(grade);
            EUnitType targetType = GetRandomType();
            SUnitSpawnRequestData data = new SUnitSpawnRequestData(targetGrade, targetType, EPlayerSide.South);
            _mdlUnit.SpawnUnit(data);
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
