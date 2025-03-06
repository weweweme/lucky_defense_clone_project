using InGame.System;
using UnityEngine;
using Util;

namespace UI
{
    /// <summary>
    /// Unit Placement UI의 View 클래스입니다.
    /// </summary>
    public sealed class VW_UnitPlacementVisible : View
    {
        [SerializeField] private Canvas _canvas;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_UnitPlacementVisible), _canvas);
        }

        public void ShowUnitPlacementField(bool value) => _canvas.enabled = value;
    }
}
