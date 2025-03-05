using UniRx;

namespace System
{
    /// <summary>
    /// 게임 종료 시 필요한 후처리를 관리하는 매니저 클래스입니다.
    /// 게임 오버 상태를 감지하고, 관련 처리를 수행합니다.
    /// </summary>
    public sealed class GameEndManager : IDisposable
    {
        /// <summary>
        /// 구독 해제를 위한 CompositeDisposable입니다.
        /// </summary>
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        /// <summary>
        /// GameEndManager를 초기화하고, 게임 오버 이벤트를 구독합니다.
        /// </summary>
        /// <param name="rootManager">게임의 핵심 매니저 참조</param>
        public GameEndManager(RootManager rootManager)
        {
            rootManager.DataManager.GameSystem.OnGameFlow
                .Where(state => state == EGameState.GameOver) // 게임 오버 상태만 필터링
                .Subscribe(_ => HandleGameOver())
                .AddTo(_disposable);
        }

        /// <summary>
        /// 게임 오버 상태 시 호출되는 후처리 메서드입니다.
        /// </summary>
        private void HandleGameOver()
        {
            // 게임 오버 처리 로직
            UnityEngine.Debug.Log("게임 오버 처리 수행");
        }

        /// <summary>
        /// 구독 해제 및 리소스를 정리합니다.
        /// </summary>
        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
