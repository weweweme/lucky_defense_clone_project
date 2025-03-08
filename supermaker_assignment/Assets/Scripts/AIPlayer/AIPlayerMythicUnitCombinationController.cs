using System;
using CleverCrow.Fluid.BTs.Tasks;
using System.Collections.Generic;
using Model;
using UnityEngine;
using Util;

namespace AIPlayer
{
    /// <summary>
    /// AI 플레이어가 신화 유닛을 조합할 수 있는지 확인하고 수행하는 컨트롤러 클래스입니다.
    /// </summary>
    public sealed class AIPlayerMythicUnitCombinationController : MonoBehaviourBase
    {
        /// <summary>
        /// AI가 신화 유닛 조합을 수행하기 위해 필요한 유닛 조건을 정의합니다.
        /// </summary>
        private struct AIUnitCombinationChecker
        {
            public readonly EUnitType ResultUnitType;
            public readonly SUnitCombinationFlagCondition[] Conditions;

            public AIUnitCombinationChecker(EUnitType resultType, params SUnitCombinationFlagCondition[] conditions)
            {
                ResultUnitType = resultType;
                Conditions = conditions;
            }
        }
        
        /// <summary>
        /// AI 플레이어의 유닛이 배치된 노드 목록입니다.
        /// </summary>
        private UnitPlacementNode[] _northGridNodes;

        /// <summary>
        /// 조합을 위한 유닛 조건 리스트입니다.
        /// </summary>
        private readonly List<AIUnitCombinationChecker> _combinationCheckers = new List<AIUnitCombinationChecker>();
        
        /// <summary>
        /// 조합 조건을 충족한 경우 선택된 조합 조건입니다.
        /// </summary>
        private AIUnitCombinationChecker _selectedCombinationChecker;
        
        /// <summary>
        /// 조합을 수행할 유닛들이 저장된 Dictionary (Key: 유닛이 있는 노드, Value: 해당 노드가 충족한 조건 인덱스)
        /// </summary>
        private readonly Dictionary<UnitPlacementNode, int> _matchingNodes = new Dictionary<UnitPlacementNode, int>();
        
        /// <summary>
        /// 중복된 조건을 방지하기 위한 인덱스 추적 HashSet
        /// </summary>
        private readonly HashSet<int> _matchedIndices = new HashSet<int>();
        
        /// <summary>
        /// 전역 유닛 관리 모델입니다.
        /// </summary>
        private MDL_Unit _globalMdlUnit;
        
        /// <summary>
        /// AI 플레이어의 유닛 데이터 클래스입니다.
        /// </summary>
        private AIPlayerDataUnit _aiPlayerUnitData;

        public void Init(AIPlayerRoot root)
        {
            _northGridNodes = root.globalRootManager.UnitGridNodeManager.NorthGridNodes;
            _globalMdlUnit = root.globalRootManager.DataManager.Unit;
            _aiPlayerUnitData = root.dataModel.Unit;

            // AI 전용 신화 조합 조건 정의
            _combinationCheckers.Add(new AIUnitCombinationChecker(
                EUnitType.Melee,
                new SUnitCombinationFlagCondition(EUnitGrade.Common, EUnitType.Melee),
                new SUnitCombinationFlagCondition(EUnitGrade.Rare, EUnitType.Melee),
                new SUnitCombinationFlagCondition(EUnitGrade.Heroic, EUnitType.Melee)
            ));

            _combinationCheckers.Add(new AIUnitCombinationChecker(
                EUnitType.Ranged,
                new SUnitCombinationFlagCondition(EUnitGrade.Common, EUnitType.Ranged),
                new SUnitCombinationFlagCondition(EUnitGrade.Rare, EUnitType.Ranged),
                new SUnitCombinationFlagCondition(EUnitGrade.Heroic, EUnitType.Ranged)
            ));
        }

        /// <summary>
        /// 현재 AI가 신화 유닛 조합을 시도할 수 있는지 판단합니다.
        /// </summary>
        /// <returns>조합이 가능하면 true, 불가능하면 false</returns>
        public bool CanCombination()
        {
            _selectedCombinationChecker = default; // 초기화

            foreach (var checker in _combinationCheckers)
            {
                if (HasRequiredUnits(checker))
                {
                    _selectedCombinationChecker = checker; // 가능한 조합 조건 저장
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 특정 조합이 가능한지 체크하는 메서드.
        /// </summary>
        /// <param name="checker">확인할 유닛 조합 조건</param>
        /// <returns>조합 가능 여부</returns>
        private bool HasRequiredUnits(AIUnitCombinationChecker checker)
        {
            _matchingNodes.Clear();
            _matchedIndices.Clear();

            foreach (var node in _northGridNodes)
            {
                if (node.UnitGroup.IsEmpty()) continue; // 빈 노드는 건너뛰기

                // 현재 노드가 checker의 조합 조건 중 하나와 일치하는지 확인
                // 일치하는 경우 matchedIndex에 해당 조건의 인덱스를 저장
                if (!TryMatchCondition(node, checker, out int matchedIndex)) continue;

                _matchingNodes[node] = matchedIndex; // 조합 가능한 유닛이 위치한 노드 저장
                _matchedIndices.Add(matchedIndex); // 이미 매칭된 조건 인덱스 기록

                if (_matchedIndices.Count == checker.Conditions.Length) return true; // 모든 조건 충족 시 즉시 반환
            }

            _matchingNodes.Clear();
            return false;
        }

        /// <summary>
        /// 개별 노드가 조합 조건을 충족하는지 확인하는 메서드.
        /// </summary>
        /// <param name="node">확인할 유닛이 위치한 노드</param>
        /// <param name="checker">조합 조건</param>
        /// <param name="matchedIndex">일치한 조건의 인덱스 (출력값)</param>
        /// <returns>조건을 만족하면 true, 아니면 false</returns>
        private bool TryMatchCondition(UnitPlacementNode node, AIUnitCombinationChecker checker, out int matchedIndex)
        {
            for (int i = 0; i < checker.Conditions.Length; i++)
            {
                if (_matchedIndices.Contains(i)) continue; // 이미 매칭된 조건이면 스킵

                var condition = checker.Conditions[i];

                if (node.UnitGroup.UnitGrade == condition.Grade && node.UnitGroup.UnitType == condition.Type)
                {
                    matchedIndex = i; // 현재 유닛이 조건을 충족하는 경우 해당 조건의 인덱스를 반환
                    return true;
                }
            }

            matchedIndex = -1; // 정상적인 흐름에서는 사용되지 않음 (return false 시점에서 처리됨)
            return false;
        }

        /// <summary>
        /// AI가 신화 유닛 조합을 실행하는 메서드입니다.
        /// </summary>
        /// <returns>조합 성공 시 Success 반환</returns>
        public TaskStatus MythicUnitCombination()
        {
            AssertHelper.NotEqualsValue(typeof(AIPlayerMythicUnitCombinationController), _matchingNodes.Count, 0);
            AssertHelper.NotEqualsEnum(typeof(AIPlayerMythicUnitCombinationController), _selectedCombinationChecker.ResultUnitType, EUnitType.None);

            // 조합을 실행할 유닛 유형 결정 (탐색된 조건의 결과 타입 사용)
            EUnitType targetType = _selectedCombinationChecker.ResultUnitType;

            // 유닛 판매 (조합을 위한 기존 유닛 제거)
            RemoveUnitsForCombination();

            ApplyUnitCount();
            
            // 새로운 신화 유닛 소환
            SUnitSpawnRequestData spawnData = new SUnitSpawnRequestData(EUnitGrade.Mythic, targetType, EPlayerSide.North);
            _globalMdlUnit.SpawnUnit(spawnData);

            return TaskStatus.Success;
        }

        /// <summary>
        /// 조합을 위한 유닛 수량을 적용하는 메서드입니다.
        /// </summary>
        private void ApplyUnitCount()
        {
            uint currentSpawnCount = _aiPlayerUnitData.GetCurrentSpawnCount();
            
            // 조합을 위한 유닛 수 차감 (-3 + 1)
            _aiPlayerUnitData.SetCurrentSpawnCount(currentSpawnCount - 2);
        }

        /// <summary>
        /// 신화 유닛 조합 시 기존 유닛을 제거하는 메서드입니다.
        /// </summary>
        private void RemoveUnitsForCombination()
        {
            foreach (var node in _matchingNodes.Keys)
            {
                node.SubUnit(); // 기존 유닛 제거
            }
    
            _matchingNodes.Clear(); // 조합 후 매칭된 노드 초기화
        }
    }
}
