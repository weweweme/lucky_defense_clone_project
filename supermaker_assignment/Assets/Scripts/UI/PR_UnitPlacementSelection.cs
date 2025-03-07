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

        public PR_UnitPlacementSelection(MDL_UnitPlacementField mdlUnitPlacementField, VW_UnitPlacementSelection view)
        {
            AssertHelper.NotNull(typeof(PR_UnitPlacementSelection), mdlUnitPlacementField);
            AssertHelper.NotNull(typeof(PR_UnitPlacementSelection), view);

            _mdlCurrency = RootManager.Ins.DataManager.Currency;

            _mdlUnitPlacementField = mdlUnitPlacementField;
            mdlUnitPlacementField.SelectedNode
                .Subscribe(view.ShowUnitPlacementField)
                .AddTo(_disposable);
            
            view.unitSellBtn.OnClickAsObservable()
                .Subscribe(SellUnit)
                .AddTo(_disposable);
        }

        /// <summary>
        /// 해당 노드의 유닛을 판매합니다.
        /// </summary>
        private void SellUnit(UniRx.Unit _)
        {
            UnitPlacementNode selectedNode = _mdlUnitPlacementField.GetSelectedNode();
            AssertHelper.NotNull(typeof(PR_UnitPlacementSelection), selectedNode);
            
            EUnitGrade grade = selectedNode.UnitGroup.UnitGrade;
            AssertHelper.NotEqualsEnum(typeof(PR_UnitPlacementSelection), grade, EUnitGrade.None);
            
            EUnitType type = selectedNode.UnitGroup.UnitType;
            AssertHelper.NotEqualsEnum(typeof(PR_UnitPlacementSelection), type, EUnitType.None);
            
            selectedNode.SellUnit();
            // TODO: 추후 Grade와 Type에 따라 return하도록 변경.
            _mdlCurrency.SubtractGold(1);

            // 판매 처리 후 노드가 비어있으면 선택된 노드를 null로 변경
            if (selectedNode.UnitGroup.IsEmpty())
            {
                _mdlUnitPlacementField.SelectNode(null);
            }
        }
        
        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
