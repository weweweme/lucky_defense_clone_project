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

        public PR_UnitPlacementSelection(MDL_UnitPlacementField mdl, VW_UnitPlacementSelection view)
        {
            AssertHelper.NotNull(typeof(PR_UnitPlacementSelection), mdl);
            AssertHelper.NotNull(typeof(PR_UnitPlacementSelection), view);
            
            mdl.SelectedNode
                .Subscribe(view.ShowUnitPlacementField)
                .AddTo(_disposable);
        }
        
        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
