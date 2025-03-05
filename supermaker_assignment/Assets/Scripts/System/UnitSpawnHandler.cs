using UniRx;
using Util;

namespace System
{
    /// <summary>
    /// 유닛 스폰 로직을 담당하는 핸들러 클래스입니다.
    /// 게임 내 유닛 스폰 이벤트를 감지하고, 해당 이벤트 처리를 담당합니다.
    /// </summary>
    public class UnitSpawnHandler : SpawnHandlerBase
    {
        public UnitSpawnHandler(RootManager rootManager)
        {
            InitRx(rootManager);
        }
        
        protected override void InitRx(RootManager rootManager)
        {
            rootManager.DataManager.Unit.OnUnitSpawn
                .Subscribe(SpawnUnit)
                .AddTo(disposable);
        }
        
        private void SpawnUnit(SUnitSpawnRequestData data)
        {
            AssertHelper.NotEquals(typeof(EnemySpawnHandler),data.UnitGrade, EUnitGrade.None);
            AssertHelper.NotEquals(typeof(EnemySpawnHandler),data.SpawnSide, EPlayerSide.None);
            
            // TODO: type에 따라 스폰할 유닛의 데이터를 셋업하는 기능 추가
            // TODO: 풀에서 Unit의 베이스를 가져와 스폰하는 기능 추가
        }
    }
}
