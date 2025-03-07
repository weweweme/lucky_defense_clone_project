using System;
using System.Collections.Generic;
using UniRx;
using Unit;

namespace Model
{
    /// <summary>
    /// 신화 유닛 조합과 관련된 데이터를 관리하는 모델 클래스.
    /// </summary>
    public class MDL_MythicUnitCombination
    {
        // 현재 신화 유닛 조합이 가능한지 여부를 나타내는 List<Rx>
        private readonly List<UnitCombinationPossibleChecker> _combinationFlagCheckers = new List<UnitCombinationPossibleChecker>();
        public MDL_MythicUnitCombination()
        {
            UnitCombinationPossibleChecker meleeMythicalChecker = new UnitCombinationPossibleChecker(
                EUnitType.Melee,
                new SUnitCombinationFlagCondition(EUnitGrade.Common, EUnitType.Melee),
                new SUnitCombinationFlagCondition(EUnitGrade.Rare, EUnitType.Melee),
                new SUnitCombinationFlagCondition(EUnitGrade.Heroic, EUnitType.Melee)
            );
            _combinationFlagCheckers.Add(meleeMythicalChecker);
            
            UnitCombinationPossibleChecker rangedMythicalChecker = new UnitCombinationPossibleChecker(
                EUnitType.Ranged,
                new SUnitCombinationFlagCondition(EUnitGrade.Common, EUnitType.Ranged),
                new SUnitCombinationFlagCondition(EUnitGrade.Rare, EUnitType.Ranged),
                new SUnitCombinationFlagCondition(EUnitGrade.Heroic, EUnitType.Ranged)
            );
            _combinationFlagCheckers.Add(rangedMythicalChecker);
        }
        public IReadOnlyList<UnitCombinationPossibleChecker> GetCombinationFlagCheckers() => _combinationFlagCheckers;
        
        // 현재 어떤 유닛을 조합할지 보여주는 Rx
        private readonly Subject<SCurrentMythicUnitCombinationData> _onMythicUnitCombination = new Subject<SCurrentMythicUnitCombinationData>();
        public IObservable<SCurrentMythicUnitCombinationData> OnMythicUnitCombination => _onMythicUnitCombination;
        public void DisplayMythicUnitCombination(SCurrentMythicUnitCombinationData data) => _onMythicUnitCombination.OnNext(data);
    }
}
