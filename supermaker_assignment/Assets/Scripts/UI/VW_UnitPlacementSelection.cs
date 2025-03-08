using System;
using InGame.System;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    /// <summary>
    /// 유닛 관련 UI 요소(판매, 합성 등)를 관리하는 클래스입니다.
    /// </summary>
    [Serializable]
    public class UnitActionUI
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] internal Button actionButton;

        public void SetActive(bool isActive)
        {
            _canvas.enabled = isActive;
            _collider.enabled = isActive;
        }
    }

    /// <summary>
    /// Unit Attack Range와 관련된 UI의 동작을 수행하는 View 클래스입니다.
    /// </summary>
    public sealed class VW_UnitPlacementSelection : View
    {
        [SerializeField] private SpriteRenderer _attackRangeSpriteRenderer;
        [SerializeField] internal UnitActionUI unitSellUI;
        [SerializeField] internal UnitActionUI unitMergeUI;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_UnitPlacementSelection), _attackRangeSpriteRenderer);
            AssertHelper.NotNull(typeof(VW_UnitPlacementSelection), unitSellUI);
            AssertHelper.NotNull(typeof(VW_UnitPlacementSelection), unitMergeUI);
        }

        public void ShowUnitPlacementField(UnitPlacementNode node)
        {
            bool isNodeNull = node == null;
            _attackRangeSpriteRenderer.enabled = !isNodeNull;
            unitSellUI.SetActive(!isNodeNull);
            unitMergeUI.SetActive(!isNodeNull);

            if (isNodeNull) return;

            float attackRange = node.UnitGroup.GetAttackRange();
            float diameter = attackRange * 2f;
            _attackRangeSpriteRenderer.transform.localScale = new Vector3(diameter, diameter, 1f);

            transform.position = node.transform.position;
        }
    }
}
