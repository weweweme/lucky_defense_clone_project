using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using Util;

namespace System
{
    /// <summary>
    /// 유닛 배치 노드의 드래그 앤 드롭 동작을 처리하는 핸들러 클래스입니다.
    /// 드래그 시작, 드래그 중, 드래그 종료 이벤트를 감지하고,
    /// 노드 간 스왑 및 시각적 효과 처리를 담당합니다.
    /// </summary>
    public sealed class NodeDragHandler : MonoBehaviourBase, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        /// <summary>
        /// 드래그 대상이 되는 노드의 참조입니다.
        /// </summary>
        private UnitPlacementNode _node;

        /// <summary>
        /// 드래그 시작 시 원래 위치를 저장하는 변수입니다.
        /// </summary>
        private Vector3 _originalPosition;

        /// <summary>
        /// 드래그 동작에 사용할 메인 카메라 참조입니다.
        /// </summary>
        private Camera _mainCam;

        /// <summary>
        /// 초기화 시, 드래그 대상 노드 및 메인 카메라를 설정합니다.
        /// </summary>
        private void Awake()
        {
            _node = gameObject.GetComponentOrAssert<UnitPlacementNode>();
            _mainCam = Camera.main;
        }

        /// <summary>
        /// 드래그 시작 시 호출되며, 원래 위치를 저장합니다.
        /// </summary>
        /// <param name="eventData">드래그 이벤트 데이터</param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            _originalPosition = _node.transform.position;
        }

        /// <summary>
        /// 드래그 중 위치를 마우스 또는 터치 위치로 변경합니다.
        /// </summary>
        /// <param name="eventData">드래그 이벤트 데이터</param>
        public void OnDrag(PointerEventData eventData)
        {
            Vector3 worldPos = _mainCam.ScreenToWorldPoint(eventData.position);
            worldPos.z = 0;
            _node.transform.position = worldPos;

            // TODO: 드래그 중 하이라이트 표시 처리
        }

        /// <summary>
        /// 드래그 종료 시, 가장 가까운 배치 노드를 탐색하고,
        /// 적절한 경우 해당 노드와 유닛 그룹을 교환합니다.
        /// </summary>
        /// <param name="eventData">드래그 이벤트 데이터</param>
        public void OnEndDrag(PointerEventData eventData)
        {
            var targetNode = FindClosestNodeOrNull();

            if (targetNode != null && targetNode != _node)
            {
                _node.SwapWith(targetNode);
            }

            // TODO: 드래그 종료 시 하이라이트 해제 처리
        }

        /// <summary>
        /// 드래그 종료 시점에서 가장 가까운 유닛 배치 노드를 탐색하여 반환합니다.
        /// 감지된 노드가 없는 경우 null을 반환합니다.
        /// </summary>
        /// <returns>가장 가까운 유닛 배치 노드 또는 null</returns>
        [CanBeNull]
        private UnitPlacementNode FindClosestNodeOrNull()
        {
            Vector3 screenPos = _mainCam.WorldToScreenPoint(_node.transform.position);
            Ray ray = _mainCam.ScreenPointToRay(screenPos);

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 0.5f, LayerMask.GetMask("UnitPlacementNode"));
            if (hit.collider != null)
            {
                return hit.collider.GetComponent<UnitPlacementNode>();
            }

            return null;
        }
    }
}
