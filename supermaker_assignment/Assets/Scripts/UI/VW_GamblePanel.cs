using InGame.System;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    /// <summary>
    /// 도박 패널을 관리하는 View 클래스입니다.
    /// </summary>
    public sealed class VW_GamblePanel : View
    {
        [SerializeField] internal Button exitBackgroundPanel;
        [SerializeField] internal Button exitButton;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_GamblePanel), exitBackgroundPanel);
            AssertHelper.NotNull(typeof(VW_GamblePanel), exitButton);
        }
    }
}
