using System;
using CleverCrow.Fluid.BTs.Tasks;
using Model;
using Unit;
using Util;

namespace AIPlayer
{
    /// <summary>
    /// AI 플레이어가 유닛을 합성할 수 있는지 확인하고, 합성 실행을 담당하는 컨트롤러 클래스입니다.
    /// </summary>
    public sealed class AIPlayerMergeController : MonoBehaviourBase
    {
        /// <summary>
        /// AI 플레이어의 유닛이 배치된 노드 목록입니다.
        /// </summary>
        private UnitPlacementNode[] _northGridNodes;

        /// <summary>
        /// 합성이 가능한 유닛이 있는 노드입니다.
        /// </summary>
        private UnitPlacementNode _mergeNode;

        /// <summary>
        /// 전역 유닛 관리 모델입니다. 
        /// 유닛 합성이 성공하면 새로운 유닛을 생성하는 데 사용됩니다.
        /// </summary>
        private MDL_Unit _globalMdlUnit;

        /// <summary>
        /// AI 플레이어 합성 컨트롤러를 초기화합니다.
        /// </summary>
        /// <param name="root">AI 플레이어 루트 객체</param>
        public void Init(AIPlayerRoot root)
        {
            _northGridNodes = root.globalRootManager.UnitGridNodeManager.NorthGridNodes;
            _globalMdlUnit = root.globalRootManager.DataManager.Unit;
        }

        /// <summary>
        /// 현재 AI가 유닛을 합성할 수 있는지 확인합니다.
        /// </summary>
        /// <returns>합성이 가능하면 true, 불가능하면 false</returns>
        public bool CanMerge()
        {
            foreach (var elem in _northGridNodes)
            {
                UnitGroup currentUnitGroup = elem.UnitGroup;

                // 유닛이 꽉 차지 않은 경우 합성 불가능
                if (!currentUnitGroup.IsFull()) continue;
                
                // 유닛 등급이 신화 등급 미만인 경우에만 합성 가능
                bool isMergePossibleGrade = currentUnitGroup.UnitGrade < EUnitGrade.Mythic;
                if (!isMergePossibleGrade) continue;

                _mergeNode = elem;
                return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// AI 플레이어가 유닛 합성을 실행합니다.
        /// </summary>
        /// <returns>합성 성공 시 Success 반환</returns>
        public TaskStatus TryMerge()
        {
            AssertHelper.NotNull(typeof(AIPlayerMergeController), _mergeNode);
            
            EUnitGrade targetGrade = GetNextGrade(_mergeNode.UnitGroup.UnitGrade);
            AssertHelper.NotEqualsEnum(typeof(AIPlayerMergeController), targetGrade, EUnitGrade.None);
            AssertHelper.NotEqualsEnum(typeof(AIPlayerMergeController), targetGrade, EUnitGrade.Mythic);
            
            EUnitType targetType = GetRandomType();
            
            _mergeNode.BeforeMergeClearUnit();
            _mergeNode = null;
            SUnitSpawnRequestData data = new SUnitSpawnRequestData(targetGrade, targetType, EPlayerSide.North);
            _globalMdlUnit.SpawnUnit(data);
            
            return TaskStatus.Success;
        }
        
        /// <summary>
        /// 현재 등급에서 다음 등급으로 증가시킵니다.
        /// </summary>
        /// <param name="grade">현재 유닛의 등급</param>
        /// <returns>상위 등급의 유닛</returns>
        private EUnitGrade GetNextGrade(EUnitGrade grade) => grade + 1;
        
        /// <summary>
        /// 근거리 또는 원거리 유닛을 랜덤으로 선택합니다.
        /// </summary>
        /// <returns>랜덤 유닛 타입</returns>
        private EUnitType GetRandomType()
        {
            return UnityEngine.Random.Range(0f, 1f) < 0.5f ? EUnitType.Melee : EUnitType.Ranged;
        }
    }
}
