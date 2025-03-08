using System;
using CleverCrow.Fluid.BTs.Tasks;
using Util;

namespace AIPlayer
{
    /// <summary>
    /// AI 플레이어가 조합을 할 수 있는지 확인하고 수행하는 컨트롤러 클래스입니다.
    /// </summary>
    public sealed class AIPlayerMergeController : MonoBehaviourBase
    {
        private UnitPlacementNode[] _northGridNodes;

        public void Init(AIPlayerRoot root)
        {
            _northGridNodes = root.globalRootManager.UnitGridNodeManager.NorthGridNodes;
        }

        public bool CanMerge()
        {
            return false;
        }
        
        public TaskStatus TryMerge()
        {
            return TaskStatus.Success;
        }
    }
}
