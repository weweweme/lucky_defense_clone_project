using UnityEngine;
using Util;

namespace System
{
    /// <summary>
    /// 개임 내에서 사용될 매니저들의 참조와 초기화를 담당합니다.
    /// </summary>
    public sealed class GameManager : Singleton<GameManager>
    {
        [SerializeField] private DataManager _dataManager;
        public DataManager DataManager => _dataManager;
        
        private WaveManager _waveManager;

        protected override void Awake()
        {
            base.Awake();
            
            AssertHelper.NotNull(typeof(GameManager), _dataManager);
        }

        private void Start()
        {
            _waveManager = new WaveManager();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            
            _waveManager.Dispose();
        }
    }
}
