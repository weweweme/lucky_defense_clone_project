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
    public sealed class GambleChoiceItem : MonoBehaviourBase
    {
        [SerializeField] internal EUnitGrade unitGrade;
        [SerializeField] internal TextMeshProUGUI chancePercent;
        [SerializeField] internal TextMeshProUGUI tryNeedDiamond;
        [SerializeField] internal Button tryButton;

        private void Awake()
        {
            AssertHelper.NotEqualsEnum(typeof(GambleChoiceItem), unitGrade, EUnitGrade.None);
            AssertHelper.NotNull(typeof(GambleChoiceItem), chancePercent);
            AssertHelper.NotNull(typeof(GambleChoiceItem), tryNeedDiamond);
            AssertHelper.NotNull(typeof(GambleChoiceItem), tryButton);
        }
    }
}
