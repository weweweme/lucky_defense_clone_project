using System.Diagnostics.CodeAnalysis;
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

        [SerializeField] private UnitPlacementNode[] northGridNodes = new UnitPlacementNode[GRID_NODE_COUNT];
        public UnitPlacementNode[] NorthGridNodes => northGridNodes;
        
        [SerializeField] private UnitPlacementNode[] southGridNodes = new UnitPlacementNode[GRID_NODE_COUNT];
        public UnitPlacementNode[] SouthGridNodes => southGridNodes;

        /// <summary>
        /// 주어진 진영에서 배치 가능한 노드를 반환합니다.
        /// </summary>
        /// <param name="side">유닛을 배치할 진영</param>
        /// <param name="data">스폰 요청 데이터 (등급, 타입)</param>
        /// <returns>배치 가능한 노드 또는 null</returns>
        [return: NotNull]
        public UnitPlacementNode FindAvailableNode(SUnitSpawnRequestData data)
        {
            UnitPlacementNode[] targetGrid = data.SpawnSide == EPlayerSide.North ? northGridNodes : southGridNodes;

            foreach (var node in targetGrid)
            {
                AssertHelper.NotNull(typeof(UnitGridNodeManager), node);

                if (!node.CanAcceptUnit(data)) continue;
                    
                return node;
            }

            throw new InvalidOperationException($"[{nameof(UnitGridNodeManager)}] {data.SpawnSide} 진영에서 배치 가능한 노드가 없습니다.");
        }
    }
}
