using AI;
using UniRx;
using UnityEngine;
using Util;

namespace System
{
    /// <summary>
    /// 게임의 시퀀스를 관리하는 매니저 클래스입니다.
    /// </summary>
    public sealed class SequenceManager : MonoBehaviourBase
    {
        [SerializeField] private AIPlayerRoot _aiPlayerRoot;
        private RootManager _rootManager;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        private void Awake()
        {
            AssertHelper.NotNull(typeof(SequenceManager), _aiPlayerRoot);
        }

        public void Init(RootManager rootManager)
        {
            _rootManager = rootManager;
            _aiPlayerRoot.Init(rootManager);
            
            rootManager.DataManager.GameSystem.OnGameFlow
                .Where(state => state == EGameState.GameOver) // 게임 오버 상태만 필터링
                .Subscribe(_ => HandleGameOver())
                .AddTo(_disposable);
        }
        
        public void StartGame()
        {
            _rootManager.WaveManager.WaveStart();
            _aiPlayerRoot.ActivateAI();
        }
        
        /// <summary>
        /// 게임 오버 상태 시 호출되는 후처리 메서드입니다.
        /// </summary>
        private void HandleGameOver()
        {
            StopGame();
        }

        private void StopGame()
        {
            _aiPlayerRoot.DeactivateAI();
        }
        
        protected override void OnDestroy()
        {
            _disposable.Clear();
            
            base.OnDestroy();
        }
    }
}
