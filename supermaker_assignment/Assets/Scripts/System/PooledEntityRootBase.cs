using Util;

namespace System
{
    /// <summary>
    /// 풀에서 가져온 엔티티의 기본 동작을 정의하는 추상 클래스입니다.
    /// </summary>
    public abstract class PooledEntityRootBase : MonoBehaviourBase
    {
        /// <summary>
        /// 오브젝트가 풀에서 활성화될 때 초기화하는 메서드입니다.
        /// </summary>
        /// <param name="side">오브젝트가 소속될 진영</param>
        public abstract void OnTakeFromPoolInit(EPlayerSide side);
    }
}
