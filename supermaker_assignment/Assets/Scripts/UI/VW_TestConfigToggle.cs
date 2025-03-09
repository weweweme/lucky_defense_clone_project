using System;
using InGame.System;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    /// <summary>
    /// 테스트 설정 패널을 열고 닫는 토글의 View 클래스입니다.
    /// </summary>
    public sealed class VW_TestConfigToggle : View
    {
        [SerializeField] internal Button testConfigToggle;
        [SerializeField] private RectTransform _directionArrow;
        
        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_TestConfigToggle), testConfigToggle);
            AssertHelper.NotNull(typeof(VW_TestConfigToggle), _directionArrow);
        }

        public void SetArrowDirection(ETestConfigState state)
        {
            AssertHelper.NotEqualsEnum(typeof(VW_TestConfigToggle), state, ETestConfigState.None);
    
            Vector3 newScale = _directionArrow.localScale;

            switch (state)
            {
                case ETestConfigState.Open:
                    newScale.x = -1;
                    break;
                case ETestConfigState.Close:
                    newScale.x = 1;
                    break;
            }

            _directionArrow.localScale = newScale;
        }
    }
}
