using System;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    /// <summary>
    /// 신화 유닛 리스트 아이템의 클래스입니다.
    /// </summary>
    public sealed class MythicUnitListItem : MonoBehaviourBase
    {
        [SerializeField] internal Button unitButton;
        [SerializeField] internal string unitName;
        [SerializeField] internal EUnitType unitType;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(MythicUnitListItem), unitButton);
            AssertHelper.NotNull(typeof(MythicUnitListItem), unitName);
            AssertHelper.NotEqualsEnum(typeof(MythicUnitListItem), unitType, EUnitType.None);
        }
    }
}
