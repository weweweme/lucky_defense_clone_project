using System;
using Model;
using UniRx;
using Util;

namespace UI
{
    /// <summary>
    /// Unit Placement Visible와 관련된 UI의 비즈니스 로직을 담당하는 Presenter 클래스입니다.
    /// </summary>
    public sealed class PR_UnitPlacementDrag : IDisposable
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        public PR_UnitPlacementDrag(MDL_UnitPlacementField mdl, VW_UnitPlacementDrag view)
        {
            AssertHelper.NotNull(typeof(PR_UnitPlacementDrag), mdl);
            AssertHelper.NotNull(typeof(PR_UnitPlacementDrag), view);
            
            mdl.IsDragging
                .Subscribe(view.ShowUnitPlacementField)
                .AddTo(_disposable);
        }
        
        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
