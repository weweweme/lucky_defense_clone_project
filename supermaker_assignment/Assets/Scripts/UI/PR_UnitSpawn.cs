using System;
using InGame.System;
using Model;
using UniRx;
using Util;

namespace UI
{
    /// <summary>
    /// Unit Spawn UI의 Presenter 클래스입니다.
    /// </summary>
    public sealed class PR_UnitSpawn : Presenter
    {
        private MDL_Unit _mdlUnit;
        
        public override void Init(DataManager dataManager, View view)
        {
            AssertHelper.NotNull(typeof(PR_UnitSpawn), dataManager);
            
            _mdlUnit = dataManager.Unit;
            AssertHelper.NotNull(typeof(PR_UnitSpawn), _mdlUnit);
            
            VW_UnitSpawn vwUnitSpawn = view as VW_UnitSpawn;
            AssertHelper.NotNull(typeof(PR_UnitSpawn), vwUnitSpawn);

            vwUnitSpawn!.btnSpawn.OnClickAsObservable()
                .Subscribe(TrySpawnUnit)
                .AddTo(disposable);
        }

        private void TrySpawnUnit(UniRx.Unit _)
        {
            // TODO: 유닛 생성 시도 기능 구현
            SUnitSpawnRequestData data = new SUnitSpawnRequestData(EUnitGrade.Common, EPlayerSide.South);
            _mdlUnit.SpawnUnit(data);   
        }
    }
}
