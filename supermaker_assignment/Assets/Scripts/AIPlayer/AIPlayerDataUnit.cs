using System;
using Model;
using UniRx;
using Util;

namespace AIPlayer
{
    /// <summary>
    /// AI 플레이어의 유닛 관련 데이터 클래스입니다.
    /// </summary>
    public class AIPlayerDataUnit
    {
        // 유닛 스폰에 필요한 골드량
        private const uint INITIAL_SPAWN_NEEDED_GOLD = 1;
        private uint _spawnNeededGold = INITIAL_SPAWN_NEEDED_GOLD;
        public uint GetSpawnNeededGold() => _spawnNeededGold;
        public void SetSpawnNeededGold(uint value) => _spawnNeededGold = value;
        
        // 유닛 스폰 최대 가능 수량
        private const uint MAX_POSSIBLE_SPAWN_COUNT = 20;
        private uint _currentSpawnCount = 0;
        public uint GetCurrentSpawnCount() => _currentSpawnCount;
        public uint GetMaxPossibleSpawnCount() => MAX_POSSIBLE_SPAWN_COUNT;
        public bool IsSpawnPossible() => _currentSpawnCount < MAX_POSSIBLE_SPAWN_COUNT;
        public void SetCurrentSpawnCount(uint value) => _currentSpawnCount = value;
        
        public AIPlayerDataUnit(DataManager dataManager, CompositeDisposable disposable)
        {
            MDL_Unit unit = dataManager.Unit;
            AssertHelper.NotNull(typeof(AIPlayerDataModel), unit);
        } 
    }
}
