using System;
using Model;
using UI;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace Enemy
{
    /// <summary>
    /// Enemy의 Stat을 관리하는 컨트롤러 클래스입니다
    /// </summary>
    public sealed class EnemyStatController : MonoBehaviourBase
    {
        [SerializeField] private VW_EnemyHP _view;
        private readonly MDL_EnemyStat _mdl = new MDL_EnemyStat();
        private PR_EnemyHP _presenter;
        
        /// <summary>
        /// 적의 기초 최대 체력입니다.
        /// </summary>
        [SerializeField] private uint _maxHP = 100;

        /// <summary>
        /// 에너미의 현재 상태입니다.
        /// </summary>
        private EEnemyState _state = EEnemyState.None;
        
        /// <summary>
        /// EnemyRoot 객체로부터 적 관련 의존성을 관리하기 위한 참조입니다.
        /// </summary>
        private EnemyRoot _enemyRoot;  

        private void Awake()
        {
            AssertHelper.NotNull(typeof(EnemyStatController), _view);
            
            // 적 생성 시, 현재 체력을 최대 체력으로 초기화합니다.
            _presenter = new PR_EnemyHP(_mdl, _view);
        }

        /// <summary>
        /// EnemyRoot 객체를 전달받아 내부 참조를 초기화합니다.
        /// </summary>
        /// <param name="root">초기화할 때 사용할 EnemyRoot 객체</param>
        public void CreatePooledItemInit(EnemyRoot root)
        {
            _enemyRoot = root;
        }

        /// <summary>
        /// 오브젝트 풀에서 꺼내질 때 호출되는 메서드입니다.
        /// 내부 데이터 초기화를 담당합니다.
        /// </summary>
        public void SetStatsForWave()
        {
            _state = EEnemyState.Alive;

            uint currentSpawnWaveIdx = _enemyRoot.currentSpawnWaveIdx;
            uint currentWaveMaxHp = _maxHP + currentSpawnWaveIdx * 20;
            
            _mdl.SetStat(currentWaveMaxHp);
        }

        /// <summary>
        /// 데미지를 받아 체력을 감소시킵니다.
        /// 체력이 0 이하가 되면 적을 제거합니다.
        /// </summary>
        /// <param name="damage">적용할 피해량</param>
        public void TakeDamage(uint damage)
        {
            _mdl.SetCurrentHp(Math.Max(0, _mdl.Hp.Value - (int)damage));

            if (_mdl.HasHpRemaining()) return;
            
            Die();
        }

        /// <summary>
        /// 데미지를 받을 수 있음을 반환하는 메서드입니다.
        /// </summary>
        /// <returns>true 반환시 피격 가능, false일 경우 피격 불가능.</returns>
        public bool CanTakeDamage()
        {
            return _state == EEnemyState.Alive;
        }

        /// <summary>
        /// 적이 사망했을 때 호출되는 메서드입니다.
        /// </summary>
        private void Die()
        {
            _state = EEnemyState.Dead;
            
            EnemyDependencyContainer dependencyContainer = _enemyRoot.dependencyContainer;
            
            dependencyContainer.mdlEnemy.KillEnemy(EEnemyType.Common);
            dependencyContainer.mdlCurrency.AddGold((uint)Random.Range(1, 3));
            dependencyContainer.enemyBasePool.ReleaseObject(_enemyRoot);
            
            uint currentAliveEnemyCount = dependencyContainer.mdlEnemy.CurrentAliveEnemyCount.Value;
            dependencyContainer.mdlEnemy.SetCurrentEnemyCount(currentAliveEnemyCount - 1);
        }

        protected override void OnDestroy()
        {
            _presenter.Dispose();
            
            base.OnDestroy();
        }
    }
}
