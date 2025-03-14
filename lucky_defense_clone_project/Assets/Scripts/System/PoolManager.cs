using Enemy;
using UI;
using Unit;
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
        
        [SerializeField] private UnitBasePool unitBasePool;
        public UnitBasePool UnitBasePool => unitBasePool;
        
        [SerializeField] private UnitSkillPoolManager unitSkillPoolManager;
        public UnitSkillPoolManager UnitSkillPoolManager => unitSkillPoolManager;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(PoolManager), enemyBasePool);
            AssertHelper.NotNull(typeof(PoolManager), unitBasePool);
            AssertHelper.NotNull(typeof(PoolManager), unitSkillPoolManager);
        }

        public void Init(RootManager rootManager)
        {
            enemyBasePool.Init(rootManager);
            unitBasePool.Init(rootManager);
        }
    }
}
