using System;
using CleverCrow.Fluid.BTs.Tasks;
using Unit;
using Util;

namespace AIPlayer
{
    /// <summary>
    /// AI 플레이어가 조합을 할 수 있는지 확인하고 수행하는 컨트롤러 클래스입니다.
    /// </summary>
    public sealed class AIPlayerMergeController : MonoBehaviourBase
    {
        private UnitPlacementNode[] _northGridNodes;
        private UnitPlacementNode _mergeNode;

        public void Init(AIPlayerRoot root)
        {
            _northGridNodes = root.globalRootManager.UnitGridNodeManager.NorthGridNodes;
        }

        public bool CanMerge()
        {
            foreach (var elem in _northGridNodes)
            {
                UnitGroup currentUnitGroup = elem.UnitGroup;

                if (!currentUnitGroup.IsFull()) continue;
                
                bool isMergePossibleGrade = currentUnitGroup.UnitGrade < EUnitGrade.Mythic;
                if (!isMergePossibleGrade) continue;

                _mergeNode = elem;
            }
            
            return false;
        }
        
        public TaskStatus TryMerge()
        {
            return TaskStatus.Success;
        }
    }
}
