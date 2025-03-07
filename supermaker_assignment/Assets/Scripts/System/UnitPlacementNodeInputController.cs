using JetBrains.Annotations;
using Model;
using UI;
using UnityEngine;
using Util;

namespace System
{
    /// <summary>
    /// 유닛 배치 노드의 드래그 앤 드롭 및 클릭 선택 처리를 담당하는 컨트롤러 클래스입니다.
    /// </summary>
    public sealed class UnitPlacementNodeInputController : MonoBehaviourBase
    {
        private readonly MDL_UnitPlacementField _mdl = new MDL_UnitPlacementField();
        [SerializeField] private VW_UnitPlacementSelection _placementSelectionView;
        private PR_UnitPlacementSelection _attackRangePr;
        [SerializeField] private VW_UnitPlacementDrag _placementDragView;
        private PR_UnitPlacementDrag _placementDragPr;

        /// <summary>
        /// 마우스 입력 처리를 담당하는 핸들러 인스턴스입니다.
        /// </summary>
        private UnitPlacementNodeInputHandler _inputHandler;

        /// <summary>
        /// 현재 드래그 중인 유닛 배치 노드입니다.
        /// </summary>
        private UnitPlacementNode _currentClickedNode;
        
        /// <summary>
        /// 마지막으로 클릭한 유닛 배치 노드입니다.
        /// </summary>
        private UnitPlacementNode _lastClickedNode;

        /// <summary>
        /// 현재 드래그 상태 여부입니다.
        /// </summary>
        private bool _isDragging;

        /// <summary>
        /// 클릭 시작 지점 (드래그 거리 계산용)
        /// </summary>
        private Vector2 _clickedPos;

        /// <summary>
        /// 드래그 판단 거리 (픽셀 기준)
        /// </summary>
        private const float DRAG_THRESHOLD = 0.3f;

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

        private void Awake()
        {
            AssertHelper.NotNull(typeof(UnitPlacementNodeInputController), _placementSelectionView);
            AssertHelper.NotNull(typeof(UnitPlacementNodeInputController), _placementDragView);

            _inputHandler = new UnitPlacementNodeInputHandler(Camera.main);
            _inputHandler.OnLeftClickStarted -= OnLeftClickStarted;
            _inputHandler.OnLeftClickStarted += OnLeftClickStarted;
            _inputHandler.OnLeftClickCanceled -= OnLeftClickCanceled;
            _inputHandler.OnLeftClickCanceled += OnLeftClickCanceled;
        }

        private void Start()
        {
            _attackRangePr = new PR_UnitPlacementSelection(_mdl, _placementSelectionView);
            _placementDragPr = new PR_UnitPlacementDrag(_mdl, _placementDragView);
        }

        /// <summary>
        /// 마우스 클릭 시작 시 처리
        /// </summary>
        private void OnLeftClickStarted()
        {
            if (IsPointerOverUnitUI())
            {
                return;
            }
            
            UnitPlacementNode currentNode = FindClosestNodeOrNull(_currentMouseWorldPos);
            if (_lastClickedNode == null && currentNode == null)
            {
                HandleClickSelect();
                ClearSelection();
                return;
            }
            
            if (_lastClickedNode != null && _lastClickedNode != currentNode)
            {
                HandleClickSelect();
                ClearSelection();
                return;
            }
            
            _currentClickedNode = currentNode;
            _isDragging = false;
            _clickedPos = _currentMouseWorldPos;
        }

        /// <summary>
        /// 마우스 클릭 해제 시 처리
        /// </summary>
        /// <summary>
        /// 마우스 클릭 해제 시 처리
        /// </summary>
        private void OnLeftClickCanceled()
        {
            if (_currentClickedNode == null)
            {
                _lastClickedNode = null;
                goto FINALIZE;
            }

            if (!_isDragging)
            {
                if (_currentClickedNode.UnitGroup.IsEmpty())
                {
                    goto FINALIZE;
                }

                _lastClickedNode = _currentClickedNode;
                HandleClickSelect(_currentClickedNode);
            }
            else
            {
                if (_currentClickedNode.UnitGroup.IsEmpty())
                {
                    goto FINALIZE;
                }

                _lastClickedNode = _currentClickedNode;
                HandleDragEnd();
            }

            FINALIZE:
            ClearSelection();
        }

        /// <summary>
        /// 드래그 종료 시, 가장 가까운 노드와 스왑 시도
        /// </summary>
        private void HandleDragEnd()
        {
            UnitPlacementNode targetNode = FindClosestNodeOrNull(_currentMouseWorldPos, _currentClickedNode);

            if (targetNode != null)
            {
                _currentClickedNode.SwapWith(targetNode);
                _lastClickedNode = null;
            }
        }

        /// <summary>
        /// 노드 선택 처리
        /// </summary>
        private void HandleClickSelect(UnitPlacementNode node = null)
        {
            _mdl.SelectNode(node);
        }

        /// <summary>
        /// 매 프레임마다 드래그 거리 감지 및 드래그 모드 전환 처리
        /// </summary>
        private void Update()
        {
            if (_currentClickedNode == null) return;

            // 드래그 여부 판단
            if (!_isDragging)
            {
                if (_currentClickedNode.UnitGroup.IsEmpty()) return;
                
                float dragDistance = Vector2.Distance(_clickedPos, _currentMouseWorldPos);
                if (dragDistance >= DRAG_THRESHOLD)
                {
                    _isDragging = true; // 드래그로 전환
                    HandleClickSelect();
                }
            }

            if (_isDragging)
            {
                // 드래그 중 시각적 처리 (필요한 경우 추가)
                UnitPlacementNode targetNode = FindClosestNodeOrNull(_currentMouseWorldPos, _currentClickedNode);
                SUnitPlacementDragData unitPlacementDragData = new SUnitPlacementDragData(true, targetNode);
                _mdl.SetIsDragging(unitPlacementDragData);
            }
        }

        /// <summary>
        /// 지정 위치에서 가장 가까운 유닛 배치 노드 탐색
        /// </summary>
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
        /// 해당 월드 좌표에 UnitUI 레이어 오브젝트가 있는지 판별합니다.
        /// </summary>
        private bool IsPointerOverUnitUI()
        {
            const float CHECK_RADIUS = 0.01f;  // 클릭 포인트 근처 작은 영역만 검사
            Collider2D[] buffer = new Collider2D[1];

            int hitCount = Physics2D.OverlapCircleNonAlloc(_currentMouseWorldPos, CHECK_RADIUS, buffer, Layers.GetLayerMask(Layers.UNIT_UI));
            return hitCount > 0;
        }
        
        /// <summary>
        /// 선택 및 드래그 상태 초기화
        /// </summary>
        private void ClearSelection()
        {
            _currentClickedNode = null;
            _isDragging = false;
            _mdl.SetIsDragging(new SUnitPlacementDragData(false, null));
        }

        protected override void OnDestroy()
        {
            _inputHandler.OnLeftClickStarted -= OnLeftClickStarted;
            _inputHandler.OnLeftClickCanceled -= OnLeftClickCanceled;
            _inputHandler.Dispose();
            _attackRangePr.Dispose();
            _placementDragPr.Dispose();

            base.OnDestroy();
        }
    }
}
