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
        private readonly MDL_UnitPlacementField _mdl;

        public PR_UnitPlacementSelection(MDL_UnitPlacementField mdl, VW_UnitPlacementSelection view)
        {
            AssertHelper.NotNull(typeof(PR_UnitPlacementSelection), mdl);
            AssertHelper.NotNull(typeof(PR_UnitPlacementSelection), view);

            _mdl = mdl;
            mdl.SelectedNode
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
            UnitPlacementNode selectedNode = _mdl.GetSelectedNode();
            AssertHelper.NotNull(typeof(PR_UnitPlacementSelection), selectedNode);
            
            selectedNode.SellUnit();
        }
        
        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
