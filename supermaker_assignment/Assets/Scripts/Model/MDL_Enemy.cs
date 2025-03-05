using System;
using UniRx;

namespace Model
{
    /// <summary>
    /// 적 유닛과 관련된 Rx를 관리하는 모델 클래스.
    /// </summary>
    public class MDL_Enemy
    {
        // 적 유닛 스폰과 관련된 Rx
        private readonly Subject<SEnemySpawnMetaData> _onEnemySpawn = new Subject<SEnemySpawnMetaData>();
        public IObservable<SEnemySpawnMetaData> OnEnemySpawn => _onEnemySpawn;
        public void TriggerSpawnEnemy(SEnemySpawnMetaData data) => _onEnemySpawn.OnNext(data);

        // 적 유닛 사망과 관련된 Rx
        private readonly Subject<EEnemyType> _onEnemyDeath = new Subject<EEnemyType>();
        public IObservable<EEnemyType> OnEnemyDeath => _onEnemyDeath;
        public void KillEnemy(EEnemyType type) => _onEnemyDeath.OnNext(type);
    }
}
