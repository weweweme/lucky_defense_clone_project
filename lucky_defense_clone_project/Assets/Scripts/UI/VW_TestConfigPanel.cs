using System;
using InGame.System;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    /// <summary>
    /// 테스트 패널의 View 클래스입니다.
    /// </summary>
    public sealed class VW_TestConfigPanel : View
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] internal TestConfigPanelItem addGold;
        [SerializeField] internal TestConfigPanelItem addDia;
        [SerializeField] internal TestConfigPanelItem addUnitLimit;
        [SerializeField] internal TestConfigPanelItem addEnemy;
        [SerializeField] internal Button backgroundExitBut;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_TestConfigPanel), _canvas);
            AssertHelper.NotNull(typeof(VW_TestConfigPanel), addGold);
            AssertHelper.NotNull(typeof(VW_TestConfigPanel), addDia);
            AssertHelper.NotNull(typeof(VW_TestConfigPanel), addUnitLimit);
            AssertHelper.NotNull(typeof(VW_TestConfigPanel), addEnemy);
            AssertHelper.NotNull(typeof(VW_TestConfigPanel), backgroundExitBut);
        }
        
        public void SetCanvasActive(ETestConfigState state)
        {
            AssertHelper.NotEqualsEnum(typeof(VW_TestConfigPanel), state, ETestConfigState.None);
            
            bool isActive = state == ETestConfigState.Open;
            _canvas.enabled = isActive;
        }
    }
}
