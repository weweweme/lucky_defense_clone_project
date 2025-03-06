using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;

namespace System
{
    /// <summary>
    /// 유닛 배치 노드의 드래그 앤 드롭 처리를 담당하는 컨트롤러 클래스입니다.
    /// Input System을 기반으로 마우스 입력을 처리하며,
    /// 드래그 중 하이라이트 처리 및 노드 간 유닛 그룹 스왑을 지원합니다.
    /// </summary>
    public sealed class UnitPlacementNodeInputController : MonoBehaviourBase, UnitPlacementNodeInputActions.IMousePositionActions
    {
        /// <summary>
        /// Input System에서 정의한 입력 액션 인스턴스입니다.
        /// </summary>
        private UnitPlacementNodeInputActions _inputActions;

        /// <summary>
        /// 현재 드래그 중인 유닛 배치 노드입니다.
        /// </summary>
        private UnitPlacementNode _draggingNode;

        /// <summary>
        /// 메인 카메라 참조입니다.
        /// </summary>
        private Camera _mainCam;

        /// <summary>
        /// 현재 드래그 상태 여부입니다.
        /// </summary>
        private bool _isDragging;

        /// <summary>
        /// 마우스 위치의 Z 깊이값 (스크린 좌표에서 월드 좌표로 변환 시 사용)입니다.
        /// </summary>
        private float _zDepth;

        /// <summary>
        /// 실시간 마우스 위치의 월드 좌표입니다.
        /// </summary>
        private Vector2 _targetWorldPos;

        /// <summary>
        /// 노드 탐색 시 사용되는 탐색 반경입니다.
        /// </summary>
        private const float SEARCH_RADIUS = 0.5f;

        /// <summary>
        /// OverlapCircleNonAlloc용 버퍼 배열입니다.
        /// </summary>
        private readonly Collider2D[] _nodeBuffer = new Collider2D[10];
        
        /// <summary>
        /// 찾을 레이어 마스크입니다.
        /// </summary>
        private readonly int TARGET_LAYER = Layers.GetLayerMask(Layers.UNIT_PLACEMENT_NODE);

        /// <summary>
        /// 초기화 시 Input System 설정 및 카메라 참조를 가져옵니다.
        /// </summary>
        private void Awake()
        {
            _mainCam = Camera.main;
            AssertHelper.NotNull(typeof(UnitPlacementNodeInputController), _mainCam);

            _inputActions = new UnitPlacementNodeInputActions();
            _inputActions.MousePosition.SetCallbacks(this);
            _inputActions.Enable();

            _zDepth = -(_mainCam!.transform.position.z);
        }

        /// <summary>
        /// 마우스 위치가 변경될 때 호출되며, 현재 마우스 위치를 월드 좌표로 변환해 저장합니다.
        /// </summary>
        /// <param name="context">입력 이벤트 컨텍스트</param>
        public void OnMousePosition(InputAction.CallbackContext context)
        {
            Vector2 mouseScreenPos = context.ReadValue<Vector2>();
            Vector3 worldMousePos = _mainCam.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, _zDepth));

            _targetWorldPos = worldMousePos;
        }

        /// <summary>
        /// 마우스 왼쪽 클릭 이벤트 콜백입니다.
        /// 클릭 시작 시 드래그 시도, 클릭 해제 시 드래그 종료를 처리합니다.
        /// </summary>
        /// <param name="context">입력 이벤트 컨텍스트</param>
        public void OnMouseLeftClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                _isDragging = TryStartDrag();
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                HandleDragEnd();
                _isDragging = false; // 드래그 종료
            }
        }

        /// <summary>
        /// 드래그 가능한 노드가 있는지 탐색하고, 성공 시 드래그 상태로 진입합니다.
        /// </summary>
        /// <returns>드래그 시작 여부</returns>
        private bool TryStartDrag()
        {
            _draggingNode = FindClosestNodeOrNull(_targetWorldPos);
            return _draggingNode != null;
        }

        /// <summary>
        /// 드래그 종료 시, 가장 가까운 노드와 유닛 그룹을 교환하고 드래그 상태를 해제합니다.
        /// </summary>
        private void HandleDragEnd()
        {
            if (!_isDragging || _draggingNode == null) return;

            UnitPlacementNode targetNode = FindClosestNodeOrNull(_targetWorldPos, _draggingNode);

            if (targetNode != null)
            {
                _draggingNode.SwapWith(targetNode);
            }

            _draggingNode = null;
            _isDragging = false;

            // TODO: 드래그 종료 시 하이라이트 해제 처리
        }

        /// <summary>
        /// 현재 위치 기준으로 가장 가까운 유닛 배치 노드를 탐색해 반환합니다.
        /// </summary>
        /// <param name="worldPos">탐색 중심 위치</param>
        /// <param name="excludeNode">탐색에서 제외할 노드 (자기 자신 등)</param>
        /// <returns>가장 가까운 노드 또는 null</returns>
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
        /// 드래그 중 시각적 처리 및 하이라이트 처리를 위한 업데이트 루틴입니다.
        /// 실제 노드 위치 이동은 처리하지 않습니다.
        /// </summary>
        private void Update()
        {
            if (!_isDragging || _draggingNode == null) return;

            // TODO: 드래그 중 시각적 처리 (하이라이트 등)
        }
    }
}
