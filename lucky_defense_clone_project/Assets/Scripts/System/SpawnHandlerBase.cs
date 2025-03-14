using UniRx;

namespace System
{
    /// <summary>
    /// 스폰 관련 공통 기능 및 UniRx 기반 리소스 관리를 담당하는 추상 클래스입니다.
    /// 개별 스폰 핸들러 클래스는 이 클래스를 상속받아 특정 로직을 구현합니다.
    /// </summary>
    public abstract class SpawnHandlerBase : IDisposable
    {
        /// <summary>
        /// Rx 스트림 구독 및 리소스 해제를 관리하기 위한 CompositeDisposable입니다.
        /// </summary>
        protected readonly CompositeDisposable disposable = new CompositeDisposable();

        /// <summary>
        /// GameManager를 기반으로 필요한 Rx 스트림을 설정하는 추상 메서드입니다.
        /// 상속받는 클래스에서 필수로 구현해야 합니다.
        /// </summary>
        /// <param name="rootManager">게임 전체 매니저 인스턴스</param>
        protected abstract void InitRx(RootManager rootManager);

        /// <summary>
        /// IDisposable 인터페이스 구현 메서드입니다.
        /// 등록된 모든 Rx 구독 및 리소스를 해제합니다.
        /// </summary>
        public void Dispose()
        {
            disposable.Dispose();
        }
    }
}
