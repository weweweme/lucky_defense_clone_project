using UnityEngine;
using Util;

namespace Enemy
{
    public sealed class EnemyMoveController : MonoBehaviourBase
    {
        [Header("이동 관련 설정")]
        [SerializeField] private float moveSpeed = 25f;
        [SerializeField] private float nodeArrivalThreshold = 0.5f;
        
        private Transform[] _pathNodes;
        private uint _currentNodeIdx;
        private uint _nextNodeIdx;

        private Rigidbody2D _rb;
        private Vector2 _moveDirection;

        private void Awake()
        {
            _rb = gameObject.GetComponentOrAssert<Rigidbody2D>();
        }

        /// <summary>
        /// 경로 초기화 및 첫 이동 방향 설정
        /// </summary>
        public void Init(Transform[] pathNodes)
        {
            _pathNodes = pathNodes;

            _currentNodeIdx = 0;
            _nextNodeIdx = 1;

            SetDirectionToNextNode();
        }

        private void FixedUpdate()
        {
            if (_moveDirection == Vector2.zero) return;

            if (HasReachedNextNode())
            {
                MoveToNextNode();
            }

            Move();
        }

        /// <summary>
        /// 다음 노드 도착 시 현재 노드와 다음 노드 인덱스 갱신
        /// </summary>
        private void MoveToNextNode()
        {
            _currentNodeIdx = _nextNodeIdx;
            _nextNodeIdx = (_currentNodeIdx + 1) % (uint)_pathNodes.Length;
            
            SetDirectionToNextNode();
        }

        /// <summary>
        /// 다음 노드 방향 설정
        /// </summary>
        private void SetDirectionToNextNode()
        {
            Transform targetNode = _pathNodes[_nextNodeIdx];
            _moveDirection = ((Vector2)targetNode.position - (Vector2)transform.position).normalized;
        }

        /// <summary>
        /// 다음 노드에 도착했는지 확인
        /// </summary>
        private bool HasReachedNextNode()
        {
            float distance = Vector3.Distance(transform.position, _pathNodes[_nextNodeIdx].position);
            return distance < nodeArrivalThreshold;
        }

        /// <summary>
        /// 현재 이동 방향으로 적 이동 처리
        /// </summary>
        private void Move()
        {
            Vector2 currentPos = _rb.position;
            Vector2 targetPos = currentPos + _moveDirection * (moveSpeed * Time.fixedDeltaTime);
            _rb.MovePosition(targetPos);
        }
    }
}
