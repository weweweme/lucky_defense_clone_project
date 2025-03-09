using InGame.System;
using UnityEngine;
using Util;

namespace UI
{
    /// <summary>
    /// 테스트 패널의 View 클래스입니다.
    /// </summary>
    public sealed class VW_TestConfigPanel : View
    {
        [SerializeField] private Canvas _canvas;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_TestConfigPanel), _canvas);
        }
        
        public void SetCanvasActive(bool value) => _canvas.enabled = value;
    }
}
