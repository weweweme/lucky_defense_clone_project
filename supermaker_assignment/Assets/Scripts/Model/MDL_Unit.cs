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
        private bool _hasValidNodes;
        public bool HasValidNodes => _hasValidNodes;
        public void SetHasValidNodesStatus(bool value) => _hasValidNodes = value;
        
        // 유닛 스폰에 필요한 골드량
        private const uint INITIAL_SPAWN_NEEDED_GOLD = 1;
        private readonly ReactiveProperty<uint> _spawnNeededGold = new ReactiveProperty<uint>(INITIAL_SPAWN_NEEDED_GOLD);
        public IReactiveProperty<uint> SpawnNeededGold => _spawnNeededGold;
        public uint GetSpawnNeededGold() => _spawnNeededGold.Value;
        public void SetSpawnNeededGold(uint value) => _spawnNeededGold.Value = value;
        
        // 유닛 스폰 최대 가능 수량
        private const uint MAX_POSSIBLE_SPAWN_COUNT = 20;
        private readonly ReactiveProperty<uint> _currentSpawnCount = new ReactiveProperty<uint>(0);
        public IReactiveProperty<uint> CurrentSpawnCount => _currentSpawnCount;
        public uint GetCurrentSpawnCount() => _currentSpawnCount.Value;
        public uint GetMaxPossibleSpawnCount() => MAX_POSSIBLE_SPAWN_COUNT;
        public bool IsSpawnPossible() => _currentSpawnCount.Value < MAX_POSSIBLE_SPAWN_COUNT;
        public void SetCurrentSpawnCount(uint value) => _currentSpawnCount.Value = value;
    }
}
