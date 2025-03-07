using System;
using System.Collections.Generic;
using Unit;

namespace Model
{
    /// <summary>
    /// 신화 유닛 조합과 관련된 데이터를 관리하는 모델 클래스.
    /// </summary>
    public class MDL_MythicUnitCombination
    {
        // 현재 신화 유닛 조합이 가능한지 여부를 나타내는 List<Rx>
        private readonly List<UnitCombinationFlagChecker> _combinationFlagCheckers = new List<UnitCombinationFlagChecker>();
        public MDL_MythicUnitCombination()
        {
            UnitCombinationFlagChecker meleeMythicalChecker = new UnitCombinationFlagChecker(
                EUnitType.Melee,
                new SUnitCombinationFlagCondition(EUnitGrade.Common, EUnitType.Melee),
                new SUnitCombinationFlagCondition(EUnitGrade.Rare, EUnitType.Melee),
                new SUnitCombinationFlagCondition(EUnitGrade.Heroic, EUnitType.Melee)
            );
            _combinationFlagCheckers.Add(meleeMythicalChecker);
            
            UnitCombinationFlagChecker rangedMythicalChecker = new UnitCombinationFlagChecker(
                EUnitType.Ranged,
                new SUnitCombinationFlagCondition(EUnitGrade.Common, EUnitType.Ranged),
                new SUnitCombinationFlagCondition(EUnitGrade.Rare, EUnitType.Ranged),
                new SUnitCombinationFlagCondition(EUnitGrade.Heroic, EUnitType.Ranged)
            );
            _combinationFlagCheckers.Add(rangedMythicalChecker);
        }
        public IReadOnlyList<UnitCombinationFlagChecker> GetCombinationFlagCheckers() => _combinationFlagCheckers;
    }
}
