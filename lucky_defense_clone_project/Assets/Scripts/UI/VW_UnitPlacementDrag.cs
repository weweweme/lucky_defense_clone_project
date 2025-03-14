using System;
using InGame.System;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    /// <summary>
    /// 드래그 이벤트와 관련된 UI의 동작을 수행하는 View 클래스입니다.
    /// </summary>
    public sealed class VW_UnitPlacementDrag : View
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private RectTransform[] _unitPlacementNodes;
        [SerializeField] private Image _dragTargetHighlight;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_UnitPlacementDrag), _canvas);
            AssertHelper.NotNull(typeof(VW_UnitPlacementDrag), _dragTargetHighlight);
            
            const int UNIT_PLACEMENT_NODE_COUNT = 18;
            AssertHelper.EqualsValue(typeof(VW_UnitPlacementDrag), _unitPlacementNodes.Length, UNIT_PLACEMENT_NODE_COUNT);
        }

        public void ShowUnitPlacementField(SUnitPlacementDragData data)
        {
            bool isDragging = data.IsDragging;

            _canvas.enabled = isDragging;

            if (!isDragging) return;
            
            UnitPlacementNode targetNode = data.TargetNode;
            if (targetNode == null)
            {
                _dragTargetHighlight.enabled = false;
                return;
            }
            
            AssertHelper.NotEqualsValue(typeof(VW_UnitPlacementDrag), targetNode.NodeIndex, uint.MaxValue);
            _dragTargetHighlight.enabled = true;
            _dragTargetHighlight.rectTransform.position = _unitPlacementNodes[targetNode.NodeIndex].position;
        }
    }
}
