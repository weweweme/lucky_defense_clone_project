using UnityEngine;
using Util;

namespace System
{
    public sealed class UnitGridDragController : MonoBehaviourBase
    {
        private UnitPlacementNode _draggingNode;
        private Vector3 _originalPosition;
        private Camera _mainCam;
        private bool _isDragging;

        private const float SEARCH_RADIUS = 0.5f;
        private readonly Collider2D[] _nodeBuffer = new Collider2D[10];

        private void Awake()
        {
            _mainCam = Camera.main;
        }

        private void Update()
        {
            if (_isDragging)
            {
                Vector3 worldPos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
                worldPos.z = 0;
                _draggingNode.transform.position = worldPos;

                // TODO: 드래그 중 시각적 표시 (하이라이트 등)

                if (Input.GetMouseButtonUp(0))
                {
                    HandleDragEnd();
                    _isDragging = false;  // 드래그 종료
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (TryStartDrag())
                {
                    _isDragging = true;  // 드래그 시작
                }
            }
        }

        private bool TryStartDrag()
        {
            Vector3 worldPos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
            int hitCount = Physics2D.OverlapCircleNonAlloc(worldPos, SEARCH_RADIUS, _nodeBuffer, LayerMask.GetMask("UnitPlacementNode"));

            Debug.Log($"hitCount: {hitCount}");
            for (int i = 0; i < hitCount; i++)
            {
                if (_nodeBuffer[i].TryGetComponent(out UnitPlacementNode node))
                {
                    Debug.Log($"node name: {node.name}");
                    _draggingNode = node;
                    _originalPosition = node.transform.position;
                    return true;
                }
            }

            return false;
        }

        private void HandleDragEnd()
        {
            if (_draggingNode == null) return;

            UnitPlacementNode targetNode = FindClosestNodeOrNull(_draggingNode.transform.position);

            if (targetNode != null && targetNode != _draggingNode)
            {
                _draggingNode.SwapWith(targetNode);
            }

            _draggingNode = null;

            // TODO: 드래그 종료 시 하이라이트 해제 처리
        }

        private UnitPlacementNode FindClosestNodeOrNull(Vector3 position)
        {
            int hitCount = Physics2D.OverlapCircleNonAlloc(position, SEARCH_RADIUS, _nodeBuffer, LayerMask.GetMask("UnitPlacementNode"));
            UnitPlacementNode closestNode = null;
            float closestDistanceSqr = float.MaxValue;

            for (int i = 0; i < hitCount; i++)
            {
                if (_nodeBuffer[i].TryGetComponent(out UnitPlacementNode candidateNode) && candidateNode != _draggingNode)
                {
                    float distanceSqr = (candidateNode.transform.position - position).sqrMagnitude;
                    if (distanceSqr < closestDistanceSqr)
                    {
                        closestDistanceSqr = distanceSqr;
                        closestNode = candidateNode;
                    }
                }
            }

            return closestNode;
        }
    }
}
