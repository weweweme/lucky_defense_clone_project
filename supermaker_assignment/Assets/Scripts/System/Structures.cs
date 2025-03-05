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
}
