using System;
using UnityEngine;
using Util;

namespace UI
{
    /// <summary>
    /// 신화 유닛 리스트 아이템의 클래스입니다.
    /// </summary>
    public sealed class MythicUnitListItem : MonoBehaviourBase
    {
        [SerializeField] internal string _unitName;
        [SerializeField] internal EUnitType _unitType;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(MythicUnitListItem), _unitName);
            AssertHelper.NotEqualsEnum(typeof(MythicUnitListItem), _unitType, EUnitType.None);
        }
    }
}
