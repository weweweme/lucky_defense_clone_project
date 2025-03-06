using InGame.System;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    /// <summary>
    /// Unit Placement UI의 View 클래스입니다.
    /// </summary>
    public sealed class VW_UnitPlacementVisible : View
    {
        [SerializeField] private Image[] _unitPlacementImages;

        private void Awake()
        {
            const int UNIT_PLACEMENT_IMAGE_COUNT = 18;
            AssertHelper.EqualsValue(typeof(VW_UnitPlacementVisible), _unitPlacementImages.Length, UNIT_PLACEMENT_IMAGE_COUNT);
        }
    }
}
