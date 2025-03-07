using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    /// <summary>
    /// 갬블 시도 UI 클래스입니다.
    /// </summary>
    public sealed class GambleTryItem : MonoBehaviourBase
    {
        [SerializeField] internal EUnitGrade unitGrade;
        [SerializeField] internal TextMeshProUGUI chancePercent;
        [SerializeField] internal TextMeshProUGUI tryNeedDiamond;
        [SerializeField] internal Button tryButton;

        private void Awake()
        {
            AssertHelper.NotEqualsEnum(typeof(GambleTryItem), unitGrade, EUnitGrade.None);
            AssertHelper.NotNull(typeof(GambleTryItem), chancePercent);
            AssertHelper.NotNull(typeof(GambleTryItem), tryNeedDiamond);
            AssertHelper.NotNull(typeof(GambleTryItem), tryButton);
        }
    }
}
