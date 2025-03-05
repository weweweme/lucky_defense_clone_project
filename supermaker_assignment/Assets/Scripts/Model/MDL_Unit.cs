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
    }
}
