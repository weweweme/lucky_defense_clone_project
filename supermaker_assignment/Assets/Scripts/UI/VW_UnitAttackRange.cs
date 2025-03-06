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
    }
}
