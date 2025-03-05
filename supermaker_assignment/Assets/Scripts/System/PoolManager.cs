using Enemy;
using UnityEngine;
using Util;

namespace System
{
    /// <summary>
    /// 게임에서 사용되는 풀들의 참조를 관리하는 클래스입니다.
    /// </summary>
    public sealed class PoolManager : MonoBehaviourBase
    {
        [SerializeField] private EnemyBasePool enemyBasePool;
        public EnemyBasePool EnemyBasePool => enemyBasePool;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(PoolManager), enemyBasePool);
        }

        public void Init(RootManager rootManager)
        {
            enemyBasePool.Init(rootManager);
        }
    }
}
