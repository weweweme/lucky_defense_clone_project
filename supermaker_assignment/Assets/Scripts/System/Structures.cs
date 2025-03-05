namespace System
{
    /// <summary>
    /// 적 소환에 필요한 메타데이터 구조체입니다.
    /// </summary>
    public struct SEnemySpawnMetaData
    {
        private readonly EEnemyType _enemyType;
        private readonly uint _waveNumber;
        private readonly EPlayerSide _spawnSide;

        /// <summary>
        /// 소환할 적의 종류입니다.
        /// </summary>
        public EEnemyType EnemyType => _enemyType;

        /// <summary>
        /// 적이 소환되는 웨이브 번호입니다.
        /// </summary>
        public uint WaveNumber => _waveNumber;

        /// <summary>
        /// 적이 소환되는 진영입니다.
        /// </summary>
        public EPlayerSide SpawnSide => _spawnSide;

        /// <summary>
        /// 적 소환 메타데이터를 생성하는 생성자입니다.
        /// </summary>
        public SEnemySpawnMetaData(EEnemyType enemyType, uint waveNumber, EPlayerSide spawnSide)
        {
            _enemyType = enemyType;
            _waveNumber = waveNumber;
            _spawnSide = spawnSide;
        }
    }
}
