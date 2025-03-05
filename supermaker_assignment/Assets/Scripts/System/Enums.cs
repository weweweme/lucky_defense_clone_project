namespace System
{
    /// <summary>
    /// 웨이브의 진행 상태를 나타내는 열거형입니다.
    /// </summary>
    public enum EWaveStates
    {
        /// <summary>
        /// 상태 없음.
        /// </summary>
        None,

        /// <summary>
        /// 적 소환 중.
        /// </summary>
        Spawning,
        
        /// <summary>
        /// 라운드 진행 중.
        /// </summary>
        InProgress,

        /// <summary>
        /// 다음 라운드 대기 중.
        /// </summary>
        Waiting,
    }
}
