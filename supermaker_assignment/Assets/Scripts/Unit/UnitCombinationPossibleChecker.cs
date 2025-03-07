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
        /// 조건의 개수를 정의합니다. 기본적으로 3개 조건을 관리합니다.
        /// </summary>
        private const uint FLAG_COUNT = 3;

        /// <summary>
        /// 각 조건이 충족된 유닛의 수를 관리하는 카운터 배열입니다.
        /// </summary>
        private readonly int[] _flagCounts = new int[FLAG_COUNT];

        /// <summary>
        /// 각 조건의 등급 및 타입 정보를 보관하는 배열입니다.
        /// </summary>
        private readonly SUnitCombinationFlagCondition[] _conditions = new SUnitCombinationFlagCondition[FLAG_COUNT];

        /// <summary>
        /// 각 배치 노드가 어떤 조건과 매칭되어 있는지를 기록하는 매핑 테이블입니다.
        /// 노드 제거 시 어떤 조건 카운트를 감소시켜야 하는지 판단할 때 사용합니다.
        /// </summary>
        private readonly Dictionary<UnitPlacementNode, int> _nodeConditionMap = new Dictionary<UnitPlacementNode, int>();

        /// <summary>
        /// 3개의 조합 조건을 초기화하는 생성자입니다.
        /// </summary>
        /// <param name="first">첫 번째 조합 조건</param>
        /// <param name="second">두 번째 조합 조건</param>
        /// <param name="third">세 번째 조합 조건</param>
        public UnitCombinationPossibleChecker(EUnitType resultType, SUnitCombinationFlagCondition first, SUnitCombinationFlagCondition second, SUnitCombinationFlagCondition third)
        {
            AssertHelper.NotEqualsEnum(typeof(UnitCombinationPossibleChecker), resultType, EUnitType.None);
            
            ResultUnitType = resultType;
            _conditions[0] = first;
            _conditions[1] = second;
            _conditions[2] = third;
        }

        /// <summary>
        /// 유닛이 새로 추가될 때 호출되며, 유닛의 등급과 타입이 조건과 일치하는 경우 해당 조건 카운트를 증가시킵니다.
        /// </summary>
        /// <param name="node">유닛이 추가된 노드</param>
        public void HandleAddUnit(UnitPlacementNode node)
        {
            for (int i = 0; i < _conditions.Length; ++i)
            {
                if (_conditions[i].Grade == node.UnitGroup.UnitGrade && _conditions[i].Type == node.UnitGroup.UnitType)
                {
                    ++_flagCounts[i];
                    _nodeConditionMap[node] = i;  // 어떤 노드가 어떤 조건을 충족했는지 기록
                    break;  // 한 노드는 한 조건만 만족한다고 가정
                }
            }

            UpdateCanCombine();
        }

        /// <summary>
        /// 유닛이 제거될 때 호출되며, 해당 노드가 만족시킨 조건 카운트를 감소시킵니다.
        /// </summary>
        /// <param name="node">유닛이 제거된 노드</param>
        public void HandleRemoveUnit(UnitPlacementNode node)
        {
            if (_nodeConditionMap.TryGetValue(node, out int conditionIndex))
            {
                --_flagCounts[conditionIndex];
                _nodeConditionMap.Remove(node);
            }

            UpdateCanCombine();
        }

        /// <summary>
        /// 현재 조건 충족 여부를 확인하고, 모든 조건이 충족되었을 경우 _canCombine을 true로 설정합니다.
        /// 하나라도 부족하면 false로 설정됩니다.
        /// </summary>
        private void UpdateCanCombine()
        {
            for (int i = 0; i < FLAG_COUNT; ++i)
            {
                if (_flagCounts[i] <= 0)  // 하나라도 조건 미충족 시 false
                {
                    _canCombine.Value = false;
                    return;
                }
            }

            _canCombine.Value = true;  // 모든 조건 충족 시 true
        }
        
        public SUnitCombinationFlagCondition GetCondition(int idx)
        {
            return _conditions[idx];
        }
        
        public bool IsConditionMet(int index)
        {
            return _flagCounts[index] > 0;
        }
    }
}
