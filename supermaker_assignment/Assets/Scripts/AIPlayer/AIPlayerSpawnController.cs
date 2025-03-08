using System;
using CleverCrow.Fluid.BTs.Tasks;
using Model;
using Util;

namespace AIPlayer
{
    /// <summary>
    /// AI Player의 스폰과 관련된 로직을 관리하는 컨트롤러 클래스입니다.
    /// </summary>
    public sealed class AIPlayerSpawnController : MonoBehaviourBase
    {
        private AIPlayerDataCurrency _aiPlayerDataCurrency;
        private AIPlayerDataUnit _aiPlayerUnitData;
        private MDL_Unit _mdlGlobalUnit;

        public void Init(AIPlayerRoot root)
        {
            AIPlayerDataModel dataModel = root.dataModel;
            AssertHelper.NotNull(typeof(AIPlayerSpawnController), dataModel);
            
            _aiPlayerDataCurrency = dataModel.Currency;
            _aiPlayerUnitData = dataModel.Unit;
            _mdlGlobalUnit = root.globalRootManager.DataManager.Unit;
        }

        /// <summary>
        /// AI 플레이어가 유닛을 생산할 수 있는지 확인합니다.
        /// </summary>
        /// <returns>유닛 생산이 가능하면 true, 불가능하면 false 반환</returns>
        public bool CanSpawnUnit()
        {
            if (!_aiPlayerUnitData.HasValidNodes) return false;
            if (!_aiPlayerUnitData.IsSpawnPossible()) return false;
            
            // 보유 골드가 부족한 경우 소환 중단
            uint currentSpawnNeededGold = _aiPlayerUnitData.GetSpawnNeededGold();
            uint currentAvailableGold = _aiPlayerDataCurrency.GetGold();
            return currentSpawnNeededGold <= currentAvailableGold;
        }

        /// <summary>
        /// AI 플레이어가 유닛을 생산하는 동작을 수행합니다.
        /// </summary>
        /// <returns>생산 성공 여부</returns>
        public TaskStatus TrySpawnUnit()
        {
            ConsumeSpawnCost();

            SUnitSpawnRequestData data = new SUnitSpawnRequestData(GetRandomGrade(), GetRandomType(), EPlayerSide.North);
            _mdlGlobalUnit.SpawnUnit(data);
            
            return TaskStatus.Success;
        }
        
        /// <summary>
        /// 유닛 소환 비용을 차감하고, 필요한 골드와 소환 카운트를 증가시킵니다.
        /// </summary>
        private void ConsumeSpawnCost()
        {
            uint currentSpawnNeededGold = _aiPlayerUnitData.GetSpawnNeededGold();
            _aiPlayerDataCurrency.SubtractGold(currentSpawnNeededGold);
            _aiPlayerUnitData.SetSpawnNeededGold(currentSpawnNeededGold + 1);

            uint currentSpawnCount = _aiPlayerUnitData.GetCurrentSpawnCount();
            _aiPlayerUnitData.SetCurrentSpawnCount(currentSpawnCount + 1);
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
