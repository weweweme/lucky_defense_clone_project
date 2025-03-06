namespace System
{
    /// <summary>
    /// 웨이브의 진행 상태를 나타내는 열거형입니다.
    /// </summary>
    public enum EWaveState
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
    
    /// <summary>
    /// 적의 종류를 나타내는 열거형입니다.
    /// </summary>
    public enum EEnemyType
    {
        /// <summary>
        /// 선택된 적 없음.
        /// </summary>
        None,
        
        /// <summary>
        /// 기본 적.
        /// </summary>
        Default,
    }
    
    /// <summary>
    /// 적의 상태를 나타내는 열거형입니다.
    /// </summary>
    public enum EEnemyState
    {
        /// <summary>
        /// 상태 없음.
        /// </summary>
        None,

        /// <summary>
        /// 생존 상태.
        /// </summary>
        Alive,

        /// <summary>
        /// 사망 상태.
        /// </summary>
        Dead,
    }
    
    /// <summary>
    /// 유닛의 등급을 나타내는 열거형입니다.
    /// </summary>
    public enum EUnitGrade
    {
        /// <summary>
        /// 등급 없음.
        /// </summary>
        None,

        /// <summary>
        /// 일반 등급.
        /// </summary>
        Common,

        /// <summary>
        /// 희귀 등급.
        /// </summary>
        Rare,

        /// <summary>
        /// 영웅 등급.
        /// </summary>
        Heroic,

        /// <summary>
        /// 신화 등급.
        /// </summary>
        Mythic,
    }
    
    /// <summary>
    /// 유닛의 유형을 나타내는 열거형입니다.
    /// </summary>
    public enum EUnitType
    {
        /// <summary>
        /// 선택된 유형 없음.
        /// </summary>
        None,

        /// <summary>
        /// 근거리 유닛.
        /// </summary>
        Melee,

        /// <summary>
        /// 원거리 유닛.
        /// </summary>
        Ranged,
    }
    
    /// <summary>
    /// 플레이어 진영을 나타내는 열거형입니다.
    /// </summary>
    public enum EPlayerSide
    {
        /// <summary>
        /// 지정되지 않음.
        /// </summary>
        None,
    
        /// <summary>
        /// 북쪽 진영.
        /// </summary>
        North,
    
        /// <summary>
        /// 남쪽 진영.
        /// </summary>
        South,
    }
    
    /// <summary>
    /// 게임의 전체 진행 상태를 나타내는 열거형입니다.
    /// </summary>
    public enum EGameState
    {
        /// <summary>
        /// 상태 없음.
        /// </summary>
        None,

        /// <summary>
        /// 게임 진행 중.
        /// </summary>
        Playing,

        /// <summary>
        /// 게임 오버.
        /// </summary>
        GameOver,

        /// <summary>
        /// 게임 클리어.
        /// </summary>
        GameClear,

        /// <summary>
        /// 게임 일시정지.
        /// </summary>
        Paused,
    }
}
