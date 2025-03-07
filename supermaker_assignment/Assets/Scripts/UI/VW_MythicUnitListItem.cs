using System;
using InGame.System;
using UnityEngine;
using Util;

namespace UI
{
    /// <summary>
    /// 신화 유닛 리스트 아이템의 View 클래스입니다.
    /// </summary>
    public sealed class VW_MythicUnitListItem : View
    {
        [SerializeField] internal string _unitName;
        [SerializeField] internal EUnitGrade _unitGrade;
        [SerializeField] internal EUnitType _unitType;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_MythicUnitListItem), _unitName);
            AssertHelper.NotEqualsEnum(typeof(VW_MythicUnitListItem), _unitGrade, EUnitGrade.None);
            AssertHelper.NotEqualsEnum(typeof(VW_MythicUnitListItem), _unitType, EUnitType.None);
        }
    }
}
