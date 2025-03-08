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
        
        public override void Init(DataManager dataManager, View view)
        {
            AssertHelper.NotNull(typeof(PR_UnitSpawn), dataManager);
            
            _mdlUnit = dataManager.Unit;
            AssertHelper.NotNull(typeof(PR_UnitSpawn), _mdlUnit);
            
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
        /// 버튼 클릭 이벤트로 호출되며, 소환 가능 여부를 확인 후 소환을 진행합니다.
        /// </summary>
        /// <param name="_">버튼 클릭 이벤트에서 전달되는 파라미터 (사용하지 않음)</param>
        private void TrySpawnUnit(UniRx.Unit _)
        {
            if (!IsPossibleSpawn()) return;

            ConsumeSpawnCost();

            SUnitSpawnRequestData data = new SUnitSpawnRequestData(GetRandomGrade(), GetRandomType(), EPlayerSide.South);
            _mdlUnit.SpawnUnit(data);
        }

        /// <summary>
        /// 유닛 소환이 가능한지 여부를 판단합니다.
        /// 1. 현재 소환이 가능한 상태인지 확인
        /// 2. 보유 골드가 충분한지 확인
        /// </summary>
        /// <returns>소환이 가능하면 true, 불가능하면 false</returns>
        private bool IsPossibleSpawn()
        {
            if (!_mdlUnit.IsSpawnPossible()) return false;

            // 보유 골드가 부족한 경우 소환 중단
            uint currentSpawnNeededGold = _mdlUnit.GetSpawnNeededGold();
            uint currentAvailableGold = _mdlCurrency.GetGold();
            return currentSpawnNeededGold <= currentAvailableGold;
        }

        /// <summary>
        /// 유닛 소환 비용을 차감하고, 필요한 골드와 소환 카운트를 증가시킵니다.
        /// </summary>
        private void ConsumeSpawnCost()
        {
            uint currentSpawnNeededGold = _mdlUnit.GetSpawnNeededGold();
            _mdlCurrency.SubGold(currentSpawnNeededGold);
            _mdlUnit.SetSpawnNeededGold(currentSpawnNeededGold + 1);

            uint currentSpawnCount = _mdlUnit.GetCurrentSpawnCount();
            _mdlUnit.SetCurrentSpawnCount(currentSpawnCount + 1);
        }
        
        private EUnitGrade GetRandomGrade()
        {
            int roll = UnityEngine.Random.Range(0, 100);

            if (roll < 50) return EUnitGrade.Common;   // 50%
            if (roll < 80) return EUnitGrade.Rare;     // 30%
            if (roll < 95) return EUnitGrade.Heroic;   // 15%
            return EUnitGrade.Mythic;                   // 5%
        }

        private EUnitType GetRandomType()
        {
            return UnityEngine.Random.Range(0f, 1f) < 0.5f ? EUnitType.Melee : EUnitType.Ranged;
        }
    }
}
