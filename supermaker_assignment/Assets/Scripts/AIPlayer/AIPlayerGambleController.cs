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
            _mdlGlobalUnit = root.rootDataManager.Unit;
        }

        /// <summary>
        /// 현재 AI가 도박을 시도할 수 있는지 판단합니다.
        /// </summary>
        /// <returns>도박이 가능하면 true, 불가능하면 false 반환</returns>
        public bool CanGamble()
        {
            if (!_aiPlayerUnitData.IsSpawnPossible()) return false;
            
            // 신화 뽑기가 가능한지 확인. 신화 뽑기가 가능한 다이아가 없다면 시도조차 하지 않음
            if (!GAMBLE_META_DATA.TryGetValue(EUnitGrade.Mythic, out SGambleMetaData metaData)) return false;
            
            uint currentAvailableDia = _aiPlayerDataCurrency.GetDiamond();
            return currentAvailableDia >= metaData.RequiredDia;
        }
        
        /// <summary>
        /// AI가 도박을 실행하는 메서드입니다.
        /// </summary>
        /// <returns>성공 시 Success, 실패 시 Failure 반환</returns>
        public TaskStatus TryGamble()
        {
            // TODO: AI가 도박을 수행하는 로직 구현
            // 1. 도박 비용 차감 (재화를 사용할 수 있는지 확인)
            // 2. 확률적으로 유닛 획득 여부 결정
            // 3. 획득 시 유닛 소환 (AIPlayerSpawnController 활용 가능)
            // 4. 실패 시 아무런 보상을 받지 않음

            return TaskStatus.Success; // 실제 결과에 따라 반환 값 변경
        }
    }
}
