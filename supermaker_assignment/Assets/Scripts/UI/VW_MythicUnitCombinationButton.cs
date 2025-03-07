using InGame.System;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    /// <summary>
    /// 신화 조합 패널을 여는 버튼의 View 클래스입니다.
    /// </summary>
    public class VW_MythicUnitCombinationButton : View
    {
        [SerializeField] internal Button openPanel;
        
        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_MythicUnitCombinationButton), openPanel);
        }
    }
}
