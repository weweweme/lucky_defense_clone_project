namespace System
{
    /// <summary>
    /// 적 소환에 필요한 메타데이터 구조체입니다.
    /// </summary>
    public struct SEnemySpawnRequestData
    {
        private readonly EnemySpawnMetaData _spawnMetaData;
        private readonly EPlayerSide _spawnSide;

        /// <summary>
        /// 소환할 적의 메타데이터 입니다.
        /// </summary>
        public EnemySpawnMetaData SpawnMetaData => _spawnMetaData;

        /// <summary>
        /// 적이 소환되는 진영입니다.
        /// </summary>
        public EPlayerSide SpawnSide => _spawnSide;

        /// <summary>
        /// 적 소환 메타데이터를 생성하는 생성자입니다.
        /// </summary>
        public SEnemySpawnRequestData(EnemySpawnMetaData spawnMetaData, EPlayerSide spawnSide)
        {
            _spawnMetaData = spawnMetaData;
            _spawnSide = spawnSide;
        }
    }
    
    /// <summary>
    /// 유닛 소환에 필요한 메타데이터 구조체입니다.
    /// </summary>
    public struct SUnitSpawnRequestData
    {
        private readonly EUnitGrade _unitGrade;
        private readonly EPlayerSide _spawnSide;

        /// <summary>
        /// 소환할 유닛의 등급입니다.
        /// </summary>
        public EUnitGrade UnitGrade => _unitGrade;

        /// <summary>
        /// 유닛이 소환되는 진영입니다.
        /// </summary>
        public EPlayerSide SpawnSide => _spawnSide;

        /// <summary>
        /// 유닛 소환 요청 데이터를 생성하는 생성자입니다.
        /// </summary>
        /// <param name="unitGrade">소환할 유닛의 등급</param>
        /// <param name="spawnSide">유닛이 소환될 진영</param>
        public SUnitSpawnRequestData(EUnitGrade unitGrade, EPlayerSide spawnSide)
        {
            _unitGrade = unitGrade;
            _spawnSide = spawnSide;
        }
    }
}
