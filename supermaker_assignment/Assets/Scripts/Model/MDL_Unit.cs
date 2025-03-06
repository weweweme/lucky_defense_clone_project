using System;
using UniRx;

namespace Model
{
    /// <summary>
    /// 유닛과 관련된 데이터를 관리하는 모델 클래스.
    /// </summary>
    public class MDL_Unit
    {
        // 유닛 스폰과 관련된 Rx
        private readonly Subject<SUnitSpawnRequestData> _onUnitSpawn = new Subject<SUnitSpawnRequestData>();
        public IObservable<SUnitSpawnRequestData> OnUnitSpawn => _onUnitSpawn;
        public void SpawnUnit(SUnitSpawnRequestData data) => _onUnitSpawn.OnNext(data);
        
        // 유닛 스폰에 필요한 골드량
        private const uint INITIAL_SPAWN_NEEDED_GOLD = 1;
        private readonly ReactiveProperty<uint> _spawnNeededGold = new ReactiveProperty<uint>(INITIAL_SPAWN_NEEDED_GOLD);
        public IReactiveProperty<uint> SpawnNeededGold => _spawnNeededGold;
        public uint GetSpawnNeededGold() => _spawnNeededGold.Value;
        public void SetSpawnNeededGold(uint value) => _spawnNeededGold.Value = value;
    }
}
