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
        private MDL_Currency _mdlCurrency;
        private uint _maxPossibleSpawnCount;
        
        public override void Init(DataManager dataManager, View view)
        {
            AssertHelper.NotNull(typeof(PR_UnitSpawn), dataManager);
            
            _mdlUnit = dataManager.Unit;
            AssertHelper.NotNull(typeof(PR_UnitSpawn), _mdlUnit);
            _maxPossibleSpawnCount = _mdlUnit.GetMaxPossibleSpawnCount();
            
            _mdlCurrency = dataManager.Currency;
            AssertHelper.NotNull(typeof(PR_UnitSpawn), _mdlCurrency);
            
            VW_UnitSpawn vwUnitSpawn = view as VW_UnitSpawn;
            AssertHelper.NotNull(typeof(PR_UnitSpawn), vwUnitSpawn);
            
            vwUnitSpawn!.btnSpawn.OnClickAsObservable()
                .Subscribe(TrySpawnUnit)
                .AddTo(disposable);
        }

        /// <summary>
        /// 유닛 소환 시도 메서드입니다.
        /// 버튼 클릭 이벤트로 호출되며, 소환에 필요한 골드와 현재 보유 골드를 비교하여 소환 가능 여부를 판단합니다.
        /// </summary>
        /// <param name="_">버튼 클릭 이벤트에서 전달되는 파라미터 (사용하지 않음)</param>
        private void TrySpawnUnit(UniRx.Unit _)
        {
            // 소환 가능 여부 판단
            uint currentSpawnCount = _mdlUnit.GetCurrentSpawnCount();
            if (currentSpawnCount >= _maxPossibleSpawnCount) return;
            
            // 보유 골드가 부족한 경우 소환 중단
            uint currentSpawnNeededGold = _mdlUnit.GetSpawnNeededGold();
            uint currentAvailableGold = _mdlCurrency.GetGold();
            if (currentSpawnNeededGold > currentAvailableGold) return;
            
            _mdlCurrency.SubtractGold(currentSpawnNeededGold);
            _mdlUnit.SetSpawnNeededGold(currentSpawnNeededGold + 1);
            _mdlUnit.SetCurrentSpawnCount(currentSpawnCount + 1);
            
            SUnitSpawnRequestData data = new SUnitSpawnRequestData(EUnitGrade.Common, EUnitType.Melee, EPlayerSide.South);
            _mdlUnit.SpawnUnit(data);   
        }
    }
}
