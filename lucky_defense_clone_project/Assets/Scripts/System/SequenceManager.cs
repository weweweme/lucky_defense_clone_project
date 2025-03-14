using Model;
using UniRx;
using Util;

namespace System
{
    /// <summary>
    /// 게임의 시퀀스를 관리하는 매니저 클래스입니다.
    /// </summary>
    public sealed class SequenceManager : MonoBehaviourBase
    {
        private RootManager _rootManager;
        private MDL_GameSystem _mdlSystem;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public void Init(RootManager rootManager)
        {
            _rootManager = rootManager;
            _mdlSystem = rootManager.DataManager.GameSystem;
            rootManager.AIPlayerRoot.Init(rootManager);
            
            _mdlSystem.OnGameFlow
                .Where(state => state == EGameState.GameOver)
                .Subscribe(_ => HandleGameOver())
                .AddTo(_disposable);
            
            _mdlSystem.OnGameFlow
                .Where(state => state == EGameState.Start)
                .Subscribe(_ => StartGame())
                .AddTo(_disposable);
        }
        
        private void StartGame()
        {
            _mdlSystem.ChangeGameFlow(EGameState.Playing);
            _rootManager.AIPlayerRoot.ActivateAI();
            
            Observable.Timer(TimeSpan.FromSeconds(5))
                .Subscribe(_ => _rootManager.WaveManager.WaveStart())
                .AddTo(_disposable);
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
            _rootManager.AIPlayerRoot.DeactivateAI();
        }
        
        protected override void OnDestroy()
        {
            _disposable.Clear();
            
            base.OnDestroy();
        }
    }
}
