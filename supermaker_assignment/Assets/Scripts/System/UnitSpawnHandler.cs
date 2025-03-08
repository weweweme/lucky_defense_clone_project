using Model;
using UniRx;
using Unit;
using Util;

namespace System
{
    /// <summary>
    /// 유닛 스폰 로직을 담당하는 핸들러 클래스입니다.
    /// 게임 내 유닛 스폰 이벤트를 감지하고, 해당 이벤트 처리를 담당합니다.
    /// </summary>
    public sealed class UnitSpawnHandler : SpawnHandlerBase
    {
        private readonly UnitBasePool _unitBasePool;
        private readonly UnitGridNodeManager _unitGridNodeManager;
        private MDL_Unit _mdlUnit;
        private MDL_MythicUnitCombination _mdlMythicUnitCombination;
        
        public UnitSpawnHandler(RootManager rootManager)
        {
            _unitBasePool = rootManager.PoolManager.UnitBasePool;
            AssertHelper.NotNull(typeof(UnitSpawnHandler), _unitBasePool);
            
            _unitGridNodeManager = rootManager.UnitGridNodeManager;
            AssertHelper.NotNull(typeof(UnitSpawnHandler), _unitGridNodeManager);
            
            InitRx(rootManager);
        }
        
        protected override void InitRx(RootManager rootManager)
        {
            var dataManager = rootManager.DataManager;
            
            _mdlUnit = dataManager.Unit;
            _mdlUnit.OnUnitSpawn
                .Subscribe(SpawnUnit)
                .AddTo(disposable);
            
            _mdlMythicUnitCombination = dataManager.MythicUnitCombination;
        }
        
        private void SpawnUnit(SUnitSpawnRequestData data)
        {
            AssertHelper.NotEqualsEnum(typeof(EnemySpawnHandler),data.UnitGrade, EUnitGrade.None);
            AssertHelper.NotEqualsEnum(typeof(EnemySpawnHandler),data.UnitType, EUnitType.None);
            AssertHelper.NotEqualsEnum(typeof(EnemySpawnHandler),data.SpawnSide, EPlayerSide.None);
            
            UnitRoot unit = _unitBasePool.GetObject();
            AssertHelper.NotNull(typeof(UnitSpawnHandler), unit);
            unit.SetupUnitClassification(data.UnitGrade, data.UnitType);
            unit.OnTakeFromPoolInit(data.SpawnSide);
            
            UnitPlacementNode placementNode = _unitGridNodeManager.FindAvailableNode(data);
            AssertHelper.NotNull(typeof(UnitSpawnHandler), placementNode);
            placementNode.AddUnit(unit);

            // 만약 유저의 유닛 스폰이 아니라면 더이상 진행하지 않습니다.
            if (data.SpawnSide == EPlayerSide.South) return;
            
            foreach (var elem in _mdlMythicUnitCombination.GetCombinationFlagCheckers())
            {
                elem.HandleAddUnit(placementNode);
            }
        }
    }
}
