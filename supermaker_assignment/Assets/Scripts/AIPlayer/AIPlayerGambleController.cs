using System;
using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Tasks;
using Model;
using Util;

namespace AIPlayer
{
    /// <summary>
    /// AI Player의 도박과 관련된 로직을 관리하는 컨트롤러 클래스입니다.
    /// </summary>
    public sealed class AIPlayerGambleController : MonoBehaviourBase
    {
        private static readonly Dictionary<EUnitGrade, SGambleMetaData> GAMBLE_META_DATA = new()
        {
            { EUnitGrade.Rare, new SGambleMetaData(EUnitGrade.Rare, 0.6f, 1) },
            { EUnitGrade.Heroic, new SGambleMetaData(EUnitGrade.Heroic, 0.2f, 1) },
            { EUnitGrade.Mythic, new SGambleMetaData(EUnitGrade.Mythic, 0.1f, 2) }
        };
        
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
        /// 현재 AI가 도박을 시도할 수 있는지 판단합니다.
        /// </summary>
        /// <returns>도박이 가능하면 true, 불가능하면 false 반환</returns>
        public bool CanGamble()
        {
            if (!_aiPlayerUnitData.HasValidNodes) return false;
            if (!_aiPlayerUnitData.IsSpawnPossible()) return false;
            
            // 신화 뽑기가 가능한지 확인. 신화 뽑기가 가능한 다이아가 없다면 시도조차 하지 않음
            if (!GAMBLE_META_DATA.TryGetValue(EUnitGrade.Mythic, out SGambleMetaData metaData)) return false;
            
            uint currentAvailableDia = _aiPlayerDataCurrency.GetDiamond();
            return currentAvailableDia >= metaData.RequiredDia;
        }
        
        /// <summary>
        /// AI가 도박을 실행하는 메서드입니다.
        /// </summary>
        public TaskStatus TryGamble()
        {
            EUnitGrade tryTargetGrade = GetRandomGambleGrade();
            ConsumeGambleCost(tryTargetGrade);
            float targetSuccessProbability = GetGambleSuccessProbability(tryTargetGrade);
            bool isSuccess = UnityEngine.Random.Range(0f, 1f) < targetSuccessProbability;
            if (!isSuccess) return TaskStatus.Success;

            SUnitSpawnRequestData data = new SUnitSpawnRequestData(tryTargetGrade, GetRandomType(), EPlayerSide.North);
            _mdlGlobalUnit.SpawnUnit(data);
            return TaskStatus.Success;
        }
        
        /// <summary>
        /// 도박 비용을 차감합니다.
        /// </summary>
        /// <param name="grade">어떤 등급 도박을 수행했는지 알 수 있는 등급</param>
        private void ConsumeGambleCost(EUnitGrade grade)
        {
            AssertHelper.NotEqualsEnum(typeof(AIPlayerGambleController), grade, EUnitGrade.None);
            AssertHelper.NotEqualsEnum(typeof(AIPlayerGambleController), grade, EUnitGrade.Common);

            if (GAMBLE_META_DATA.TryGetValue(grade, out SGambleMetaData metaData))
            {
                _aiPlayerDataCurrency.SubDiamond(metaData.RequiredDia);
            }
        }
        
        /// <summary>
        /// AI가 어떤 등급의 도박을 선택할지 확률적으로 결정합니다.
        /// </summary>
        /// <returns>선택된 도박 등급</returns>
        private EUnitGrade GetRandomGambleGrade()
        {
            float randomValue = UnityEngine.Random.Range(0f, 1f);
            float cumulativeProbability = 0f;

            foreach (var kvp in GAMBLE_META_DATA)
            {
                cumulativeProbability += kvp.Value.SuccessProbability;
                if (randomValue < cumulativeProbability)
                {
                    return kvp.Key;
                }
            }

            return EUnitGrade.Mythic; // 논리적으로 도달할 수 없는 경우 대비
        }
        
        /// <summary>
        /// 등급별 도박 성공 확률을 반환합니다.
        /// </summary>
        /// <param name="grade">도박 대상 유닛 등급</param>
        /// <returns>성공 확률 (0.0f ~ 1.0f)</returns>
        private float GetGambleSuccessProbability(EUnitGrade grade)
        {
            AssertHelper.NotEqualsEnum(typeof(AIPlayerGambleController), grade, EUnitGrade.None);
            AssertHelper.NotEqualsEnum(typeof(AIPlayerGambleController), grade, EUnitGrade.Common);
    
            if (GAMBLE_META_DATA.TryGetValue(grade, out SGambleMetaData metaData))
            {
                return metaData.SuccessProbability;
            }
    
            throw new ArgumentOutOfRangeException(nameof(grade), grade, "Unsupported grade for gambling.");
        }
        
        /// <summary>
        /// 근거리와 원거리를 랜덤 선택합니다.
        /// </summary>
        /// <returns>랜덤 유닛 타입</returns>
        private EUnitType GetRandomType()
        {
            float meleeRatio = 0.5f;
            
            return UnityEngine.Random.Range(0f, 1f) < meleeRatio ? EUnitType.Melee : EUnitType.Ranged;
        }
    }
}
