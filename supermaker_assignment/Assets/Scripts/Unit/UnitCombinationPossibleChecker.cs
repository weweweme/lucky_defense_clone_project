using System;
using System.Collections.Generic;
using UniRx;
using Util;

namespace Unit
{
    /// <summary>
    /// 유닛 조합 플래그를 관리하는 클래스입니다.
    /// 지정된 조건에 맞는 유닛들이 특정 개수 이상 배치되었는지를 추적하고,
    /// 모든 조건을 충족했을 때 합성 가능 상태(_canCombine)를 ReactiveProperty로 노출합니다.
    /// </summary>
    public sealed class UnitCombinationPossibleChecker
    {
        /// <summary>
        /// 합성 완료 시 생성되는 유닛의 타입입니다.
        /// </summary>
        public readonly EUnitType ResultUnitType;

        /// <summary>
        /// 현재 조합 가능한 상태인지 여부를 나타내는 ReactiveProperty입니다.
        /// 모든 조건이 충족되면 true로 설정되며, 조건이 하나라도 부족하면 false가 됩니다.
        /// </summary>
        private readonly ReactiveProperty<bool> _canCombine = new ReactiveProperty<bool>();
        public IReadOnlyReactiveProperty<bool> CanCombine => _canCombine;

        /// <summary>
        /// 조합 조건 개수 (3개 고정)
        /// </summary>
        private const uint FLAG_COUNT = 3;

        /// <summary>
        /// 각 배치 노드가 어떤 조건과 매칭되는지를 기록하는 매핑 테이블
        /// </summary>
        private readonly Dictionary<UnitPlacementNode, int> _nodeConditionMap = new Dictionary<UnitPlacementNode, int>();

        /// <summary>
        /// 유닛 조합 조건
        /// </summary>
        private readonly SUnitCombinationFlagCondition[] _conditions = new SUnitCombinationFlagCondition[FLAG_COUNT];

        public IReadOnlyDictionary<UnitPlacementNode, int> NodeConditionMap => _nodeConditionMap;

        public UnitCombinationPossibleChecker(EUnitType resultType, SUnitCombinationFlagCondition first, SUnitCombinationFlagCondition second, SUnitCombinationFlagCondition third)
        {
            AssertHelper.NotEqualsEnum(typeof(UnitCombinationPossibleChecker), resultType, EUnitType.None);

            ResultUnitType = resultType;
            _conditions[0] = first;
            _conditions[1] = second;
            _conditions[2] = third;
        }

        /// <summary>
        /// 유닛이 추가될 때 호출되며, 조합 조건을 검사 후 매핑 테이블에 저장
        /// </summary>
        public void HandleAddUnit(UnitPlacementNode node)
        {
            if (_nodeConditionMap.ContainsKey(node)) return; // 중복 추가 방지

            for (int i = 0; i < FLAG_COUNT; ++i)
            {
                if (_conditions[i].Grade == node.UnitGroup.UnitGrade && _conditions[i].Type == node.UnitGroup.UnitType)
                {
                    _nodeConditionMap[node] = i;
                    UpdateCanCombine();
                    return;
                }
            }
        }

        /// <summary>
        /// 패널이 닫힐 때 모든 데이터 초기화
        /// </summary>
        public void ClearAll()
        {
            _nodeConditionMap.Clear();
            _canCombine.Value = false;
        }

        /// <summary>
        /// 현재 조건 충족 여부를 확인하고 조합 가능 상태 갱신
        /// </summary>
        private void UpdateCanCombine()
        {
            _canCombine.Value = _nodeConditionMap.Count == FLAG_COUNT;
        }

        /// <summary>
        /// 특정 인덱스의 조건을 반환
        /// </summary>
        public SUnitCombinationFlagCondition GetCondition(int idx) => _conditions[idx];

        /// <summary>
        /// 특정 조건의 충족 여부 반환
        /// </summary>
        public bool HasRequiredUnit(int index)
        {
            int count = 0;
    
            // 해당 조건(index)을 충족하는 노드가 존재하는지 확인
            foreach (var value in _nodeConditionMap.Values)
            {
                if (value == index)
                {
                    count++;
                }
            }

            return count > 0; // 한 개라도 충족하면 true
        }
    }
}
