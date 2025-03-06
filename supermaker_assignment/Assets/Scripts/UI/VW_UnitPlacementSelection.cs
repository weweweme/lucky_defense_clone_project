using System;
using InGame.System;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    /// <summary>
    /// Unit Attack Range와 관련된 UI의 동작을 수행하는 View 클래스입니다.
    /// </summary>
    public sealed class VW_UnitPlacementSelection : View
    {
        [SerializeField] private SpriteRenderer _attackRangeSpriteRenderer;
        [SerializeField] private Canvas _unitSellCanvas;
        [SerializeField] internal Button unitSellBtn; 
        
        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_UnitPlacementSelection), _attackRangeSpriteRenderer);
            AssertHelper.NotNull(typeof(VW_UnitPlacementSelection), _unitSellCanvas);
            AssertHelper.NotNull(typeof(VW_UnitPlacementSelection), unitSellBtn);
        }

        public void ShowUnitPlacementField(UnitPlacementNode node)
        {
            bool isNodeNull = node == null;
            _attackRangeSpriteRenderer.enabled = !isNodeNull;
            _unitSellCanvas.enabled = !isNodeNull;
            
            if (isNodeNull) return;
            
            float attackRange = node.UnitGroup.GetAttackRange();
            float diameter = attackRange * 2f;
            _attackRangeSpriteRenderer.transform.localScale = new Vector3(diameter, diameter, 1f);
            
            transform.position = node.transform.position;
        }
    }
}
