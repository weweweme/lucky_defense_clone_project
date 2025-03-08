using System;
using CleverCrow.Fluid.BTs.Tasks;
using System.Collections.Generic;
using Model;
using Util;

namespace AIPlayer
{
    /// <summary>
    /// AI 플레이어가 신화 유닛을 조합할 수 있는지 확인하고 수행하는 컨트롤러 클래스입니다.
    /// </summary>
    public sealed class AIPlayerMythicUnitCombinationController : MonoBehaviourBase
    {
        /// <summary>
        /// AI 플레이어의 유닛이 배치된 노드 목록입니다.
        /// </summary>
        private UnitPlacementNode[] _northGridNodes;

        /// <summary>
        /// 조합을 위한 유닛 조건 리스트입니다.
        /// </summary>
        private readonly List<AIUnitCombinationChecker> _combinationCheckers = new();

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
        
        private MDL_Unit _globalMdlUnit;

        public void Init(AIPlayerRoot root)
        {
            _northGridNodes = root.globalRootManager.UnitGridNodeManager.NorthGridNodes;
            _globalMdlUnit = root.globalRootManager.DataManager.Unit;

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

        public bool CanCombination()
        {
            return false;
        }
        
        public TaskStatus MythicUnitCombination()
        {
            return TaskStatus.Success;
        }
    }
}
