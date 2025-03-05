using UnityEngine;
using Util;

namespace System
{
    /// <summary>
    /// 유닛이 활동하는 그리드의 노드들을 참조를 관리하는 클래스입니다.
    /// </summary>
    public class UnitGridNodeManager : MonoBehaviourBase
    {
        /// <summary>
        /// 각 그리드의 노드 개수 상수
        /// </summary>
        private const int GRID_NODE_COUNT = 18;

        [SerializeField] private Transform[] northGridNodes = new Transform[GRID_NODE_COUNT];
        public Transform[] NorthGridNodes => northGridNodes;
        
        [SerializeField] private Transform[] southGridNodes = new Transform[GRID_NODE_COUNT];
        public Transform[] SouthGridNodes => southGridNodes;
    }
}
