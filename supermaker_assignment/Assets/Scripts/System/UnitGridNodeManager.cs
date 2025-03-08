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

            UnitPlacementNode emptyNode = null; // 완전히 비어있는 노드를 저장할 변수

            foreach (var node in targetGrid)
            {
                AssertHelper.NotNull(typeof(UnitGridNodeManager), node);

                // 유닛 그룹이 비어있으면 비어있는 노드로 임시 저장
                if (node.UnitGroup.IsEmpty())
                {
                    if (emptyNode == null) emptyNode = node;
                    continue;
                }

                // 기존 노드가 스폰 요청 데이터와 일치하면 즉시 반환
                if (node.CanAcceptUnit(data))
                {
                    return node;
                }
            }

            // 기존 노드에 배치할 수 없다면 비어있는 노드 반환
            if (emptyNode != null) return emptyNode;

            throw new InvalidOperationException($"[{nameof(UnitGridNodeManager)}] {data.SpawnSide} 진영에서 배치 가능한 노드가 없습니다.");
        }
    }
}
