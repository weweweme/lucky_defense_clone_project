using UnityEngine;
using Util;

namespace System
{
    /// <summary>
    /// 개임 내에서 사용될 매니저들의 참조와 초기화를 담당합니다.
    /// </summary>
    public sealed class RootManager : Singleton<RootManager>
    {
        [SerializeField] private DataManager _dataManager;
        public DataManager DataManager => _dataManager;

        [SerializeField] private PoolManager _poolManager;
        public PoolManager PoolManager => _poolManager;

        [SerializeField] private EnemyPathNodeManager enemyPathNodeManager;
        public EnemyPathNodeManager EnemyPathNodeManager => enemyPathNodeManager;
        
        [SerializeField] private UnitGridNodeManager _unitGridNodeManager;
        public UnitGridNodeManager UnitGridNodeManager => _unitGridNodeManager;

        private readonly UIManager _uiManager = new UIManager();
        public UIManager UIManager => _uiManager;
        
        private SpawnManager _spawnManager;
        private WaveManager _waveManager;
        private GameEndManager _gameEndManager;

        protected override void Awake()
        {
            base.Awake();
            
            AssertHelper.NotNull(typeof(RootManager), _dataManager);
            AssertHelper.NotNull(typeof(RootManager), _poolManager);
            AssertHelper.NotNull(typeof(RootManager), enemyPathNodeManager);
            AssertHelper.NotNull(typeof(RootManager), _unitGridNodeManager);
        }

        private void Start()
        {
            _poolManager.Init(this);
            _waveManager = new WaveManager(this);
            _spawnManager = new SpawnManager(this);
            _gameEndManager = new GameEndManager(this);
            _uiManager.Init(_dataManager);

            _waveManager.WaveStart();
        }

        protected override void OnDispose()
        {
            _spawnManager.Dispose();
            _waveManager.Dispose();
            _gameEndManager.Dispose();
            
            base.OnDispose();
        }
    }
}
