using System;
using UniRx;

namespace Model
{
    /// <summary>
    /// 적 유닛과 관련된 데이터를 관리하는 모델 클래스.
    /// </summary>
    public class MDL_Enemy
    {
        // 적 유닛 스폰과 관련된 Rx
        private readonly Subject<SEnemySpawnRequestData> _onEnemySpawn = new Subject<SEnemySpawnRequestData>();
        public IObservable<SEnemySpawnRequestData> OnEnemySpawn => _onEnemySpawn;
        public void SpawnEnemy(SEnemySpawnRequestData data) => _onEnemySpawn.OnNext(data);

        // 적 유닛 사망과 관련된 Rx
        private readonly Subject<UniRx.Unit> _onEnemyDeath = new Subject<UniRx.Unit>();
        public IObservable<UniRx.Unit> OnEnemyDeath => _onEnemyDeath;
        public void KillEnemy() => _onEnemyDeath.OnNext(UniRx.Unit.Default);
        
        // 현재 맵에 존재하는 적 유닛의 수를 나타내는 Rx
        private readonly ReactiveProperty<uint> _currentAliveEnemyCount = new ReactiveProperty<uint>(0);
        public IReadOnlyReactiveProperty<uint> CurrentAliveEnemyCount => _currentAliveEnemyCount;
        public void SetCurrentEnemyCount(uint count) => _currentAliveEnemyCount.Value = count;
    }
}
