using JetBrains.Annotations;
using Model;
using UnityEngine;
using Util;

namespace System
{
    /// <summary>
    /// 유닛 배치 노드의 드래그 앤 드롭 처리를 담당하는 컨트롤러 클래스입니다.
    /// 실제 드래그 처리 및 노드 간 유닛 그룹 스왑 로직을 담당합니다.
    /// </summary>
    public sealed class UnitPlacementNodeInputController : MonoBehaviourBase
    {
        private readonly MDL_UnitPlacementField _mdl = new MDL_UnitPlacementField();
        
        /// <summary>
        /// 마우스 입력 처리를 담당하는 핸들러 인스턴스입니다.
        /// </summary>
        private UnitPlacementNodeInputHandler _inputHandler;

        /// <summary>
        /// 현재 드래그 중인 유닛 배치 노드입니다.
        /// </summary>
        private UnitPlacementNode _draggingNode;

        /// <summary>
        /// 현재 드래그 상태 여부입니다.
        /// </summary>
        private bool _isDragging;

        /// <summary>
        /// 현재 마우스 위치의 월드 좌표입니다.
        /// </summary>
        private Vector2 _currentMouseWorldPos => _inputHandler.CurrentMouseWorldPosition;

        /// <summary>
        /// 유닛 배치 노드 탐색 시 사용되는 반경입니다.
        /// </summary>
        private const float SEARCH_RADIUS = 0.5f;

        /// <summary>
        /// OverlapCircleNonAlloc에 사용할 탐색 결과 버퍼입니다.
        /// </summary>
        private readonly Collider2D[] _nodeBuffer = new Collider2D[10];

        /// <summary>
        /// 유닛 배치 노드 탐색에 사용할 레이어 마스크입니다.
        /// </summary>
        private readonly int TARGET_LAYER = Layers.GetLayerMask(Layers.UNIT_PLACEMENT_NODE);

        /// <summary>
        /// 초기화 시 입력 핸들러를 생성하고, 클릭 이벤트를 등록합니다.
        /// </summary>
        private void Awake()
        {
            _inputHandler = new UnitPlacementNodeInputHandler(Camera.main);
            _inputHandler.OnLeftClickStarted -= TryStartDrag;
            _inputHandler.OnLeftClickStarted += TryStartDrag;
            _inputHandler.OnLeftClickCanceled -= HandleDragEnd;
            _inputHandler.OnLeftClickCanceled += HandleDragEnd;
        }

        /// <summary>
        /// 드래그 시작 시도 메서드입니다.
        /// 현재 마우스 위치에서 가장 가까운 유닛 배치 노드를 탐색하고,
        /// 드래그 가능한 노드가 존재할 경우 드래그 상태로 전환합니다.
        /// </summary>
        private void TryStartDrag()
        {
            _draggingNode = FindClosestNodeOrNull(_currentMouseWorldPos);
            _isDragging = _draggingNode != null;
        }

        /// <summary>
        /// 드래그 종료 시 처리 메서드입니다.
        /// 드래그 종료 시 가장 가까운 유닛 배치 노드와의 스왑을 시도하고,
        /// 드래그 상태를 해제합니다.
        /// </summary>
        private void HandleDragEnd()
        {
            if (!_isDragging || _draggingNode == null) return;

            UnitPlacementNode targetNode = FindClosestNodeOrNull(_currentMouseWorldPos, _draggingNode);

            if (targetNode != null)
            {
                _draggingNode.SwapWith(targetNode);
            }

            _draggingNode = null;
            _isDragging = false;

            // TODO: 드래그 종료 시 하이라이트 해제 처리
        }

        /// <summary>
        /// 지정된 위치에서 가장 가까운 유닛 배치 노드를 탐색해 반환합니다.
        /// 특정 노드는 탐색 대상에서 제외할 수 있습니다.
        /// </summary>
        /// <param name="worldPos">탐색 중심 위치</param>
        /// <param name="excludeNode">탐색에서 제외할 노드 (자기 자신 등)</param>
        /// <returns>가장 가까운 유닛 배치 노드 또는 null</returns>
        [CanBeNull]
        private UnitPlacementNode FindClosestNodeOrNull(Vector2 worldPos, UnitPlacementNode excludeNode = null)
        {
            int hitCount = Physics2D.OverlapCircleNonAlloc(worldPos, SEARCH_RADIUS, _nodeBuffer, TARGET_LAYER);
            UnitPlacementNode closestNode = null;
            float closestDistanceSqr = float.MaxValue;

            for (int i = 0; i < hitCount; i++)
            {
                if (!_nodeBuffer[i].TryGetComponent(out UnitPlacementNode candidateNode) || candidateNode == excludeNode)
                    continue;

                float distanceSqr = ((Vector2)candidateNode.transform.position - worldPos).sqrMagnitude;
                if (distanceSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqr;
                    closestNode = candidateNode;
                }
            }

            return closestNode;
        }

        /// <summary>
        /// 드래그 중 시각적 처리(하이라이트 등)를 위한 업데이트 메서드입니다.
        /// (위치 변경은 별도로 처리하지 않음)
        /// </summary>
        private void Update()
        {
            if (!_isDragging || _draggingNode == null) return;

            // TODO: 드래그 중 시각적 처리 (하이라이트 등)
        }

        /// <summary>
        /// 오브젝트가 파괴될 때 입력 핸들러의 이벤트 해제 및 리소스 정리를 수행합니다.
        /// </summary>
        protected override void OnDestroy()
        {
            _inputHandler.OnLeftClickStarted -= TryStartDrag;
            _inputHandler.OnLeftClickCanceled -= HandleDragEnd;
            _inputHandler.Dispose();

            base.OnDestroy();
        }
    }
}
