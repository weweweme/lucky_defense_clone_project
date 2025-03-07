using System;
using System.Collections.Generic;
using UniRx;
using Unit;

namespace Model
{
    /// <summary>
    /// UnitPlacementNode와 관련된 데이터를 관리하는 모델 클래스.
    /// </summary>
    public class MDL_UnitPlacementField
    {
        // 현재 화면에 선택된 노드와 관련된 Rx
        private readonly BehaviorSubject<UnitPlacementNode> _selectedNode = new BehaviorSubject<UnitPlacementNode>(null);
        public IObservable<UnitPlacementNode> SelectedNode => _selectedNode;
        public void SelectNode(UnitPlacementNode node) => _selectedNode.OnNext(node);
        public UnitPlacementNode GetSelectedNode() => _selectedNode.Value;
        
        // 현재 드래그 중인지 Rx
        private readonly Subject<SUnitPlacementDragData> _isDragging = new Subject<SUnitPlacementDragData>();
        public IObservable<SUnitPlacementDragData> IsDragging => _isDragging;
        public void SetIsDragging(SUnitPlacementDragData value) => _isDragging.OnNext(value);
        
        // 현재 신화 유닛 조합이 가능한지 여부를 나타내는 List<Rx>
        private readonly List<UnitCombinationFlagChecker> _combinationFlagCheckers = new List<UnitCombinationFlagChecker>();
        public MDL_UnitPlacementField()
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
