namespace System
{
    /// <summary>
    /// 적 소환에 필요한 메타데이터를 관리하는 클래스입니다.
    /// </summary>
    public sealed class EnemySpawnMetaData
    {
        /// <summary>
        /// 소환할 적의 종류입니다.
        /// </summary>
        public EEnemyType EnemyType { get; private set; }

        /// <summary>
        /// 적이 소환되는 웨이브 번호입니다.
        /// </summary>
        public uint WaveNumber { get; private set; }

        /// <summary>
        /// 적 소환 메타데이터를 설정하는 메서드입니다.
        /// </summary>
        /// <param name="enemyType">적 종류</param>
        /// <param name="waveNumber">웨이브 번호</param>
        /// <param name="spawnSide">소환 진영</param>
        public void SetData(EEnemyType enemyType, uint waveNumber)
        {
            EnemyType = enemyType;
            WaveNumber = waveNumber;
        }
    }
}
