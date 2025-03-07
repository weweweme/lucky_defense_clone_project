using InGame.System;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    /// <summary>
    /// 도박 버튼을 관리하는 View 클래스입니다.
    /// </summary>
    public sealed class VW_GambleButton : View
    {
        [SerializeField] internal Button gambleBut;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_GambleButton), gambleBut);
        }
    }
}
