using UnityEngine;
using Util;

namespace System
{
    /// <summary>
    /// 적의 이동 경로를 저장하고 제공하는 클래스.
    /// </summary>
    /// <remarks>
    /// 이 클래스는 적이 이동해야 할 경로(패스포인트)들을 저장하며, 
    /// 참조를 제공한다.
    /// </remarks>
    public sealed class EnemyPathManager : MonoBehaviourBase
    {
        /// <summary>
        /// 각 경로의 노드 개수 상수
        /// </summary>
        private const int PATH_NODE_COUNT = 4;

        [SerializeField] private Transform[] northPathNodes = new Transform[PATH_NODE_COUNT];
        public Transform[] NorthPathNodes => northPathNodes;
        
        [SerializeField] private Transform[] southPathNodes = new Transform[PATH_NODE_COUNT];
        public Transform[] SouthPathNodes => southPathNodes;
    }
}
