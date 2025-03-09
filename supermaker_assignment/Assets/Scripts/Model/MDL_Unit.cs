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
        
        // 현재 유효한 노드가 있는지 여부
        private bool _hasValidNodes = true;
        public bool HasValidNodes => _hasValidNodes;
        public void SetHasValidNodesStatus(bool value) => _hasValidNodes = value;
        
        // 유닛 스폰에 필요한 골드량
        private const uint INITIAL_SPAWN_NEEDED_GOLD = 1;
        private readonly ReactiveProperty<uint> _spawnNeededGold = new ReactiveProperty<uint>(INITIAL_SPAWN_NEEDED_GOLD);
        public IReactiveProperty<uint> SpawnNeededGold => _spawnNeededGold;
        public uint GetSpawnNeededGold() => _spawnNeededGold.Value;
        public void SetSpawnNeededGold(uint value) => _spawnNeededGold.Value = value;
        
        // 유닛 스폰 관련 Rx
        private readonly ReactiveProperty<uint> _currentSpawnCount = new ReactiveProperty<uint>(0);
        public IReactiveProperty<uint> CurrentSpawnCount => _currentSpawnCount;
        public uint GetCurrentSpawnCount() => _currentSpawnCount.Value;
        public bool IsSpawnPossible() => _currentSpawnCount.Value < _maxPossibleSpawnCount.Value;
        public void SetCurrentSpawnCount(uint value) => _currentSpawnCount.Value = value;
        
        private const uint INIT_MAX_POSSIBLE_SPAWN_COUNT = 20;
        private readonly ReactiveProperty<uint> _maxPossibleSpawnCount = new ReactiveProperty<uint>(INIT_MAX_POSSIBLE_SPAWN_COUNT);
        public IReactiveProperty<uint> MaxPossibleSpawnCount => _maxPossibleSpawnCount;
        public uint GetMaxPossibleSpawnCount() => _maxPossibleSpawnCount.Value;
        public void SetMaxPossibleSpawnCount(uint value) => _maxPossibleSpawnCount.Value = value;
    }
}
