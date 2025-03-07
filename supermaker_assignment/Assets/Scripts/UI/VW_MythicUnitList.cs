using System.Collections.Generic;
using InGame.System;
using UnityEngine;
using Util;

namespace UI
{
    /// <summary>
    /// 신화 유닛 리스트의 View 클래스입니다.
    /// </summary>
    public sealed class VW_MythicUnitList : View
    {
        [SerializeField] internal List<MythicUnitListItem> mythicUnitItemList;
        
        private void Awake()
        {
            const int CURRENT_MYTHIC_UNIT_COUNT = 2;
            AssertHelper.EqualsValue(typeof(VW_MythicUnitList), mythicUnitItemList.Count, CURRENT_MYTHIC_UNIT_COUNT);
        }
    }
}
