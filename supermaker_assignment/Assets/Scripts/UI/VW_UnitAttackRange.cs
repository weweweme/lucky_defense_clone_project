using System;
using InGame.System;
using UnityEngine;
using Util;

namespace UI
{
    /// <summary>
    /// Unit Attack Range와 관련된 UI의 동작을 수행하는 View 클래스입니다.
    /// </summary>
    public sealed class VW_UnitAttackRange : View
    {
        [SerializeField] private SpriteRenderer _attackRangeSpriteRenderer;
        
        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_UnitAttackRange), _attackRangeSpriteRenderer);
        }

        public void ShowAttackRange(UnitPlacementNode node)
        {
            if (node == null)
            {
                _attackRangeSpriteRenderer.enabled = false;
                return;
            }
            
            float attackRange = node.UnitGroup.GetAttackRange();
            float diameter = attackRange * 2f;
            transform.localScale = new Vector3(diameter, diameter, 1f);
            
            transform.position = node.transform.position;
            _attackRangeSpriteRenderer.enabled = true;
        }
    }
}
