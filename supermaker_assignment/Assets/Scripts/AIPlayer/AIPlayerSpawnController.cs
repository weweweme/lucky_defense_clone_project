using CleverCrow.Fluid.BTs.Tasks;
using Util;

namespace AIPlayer
{
    /// <summary>
    /// AI Player의 스폰과 관련된 로직을 관리하는 컨트롤러 클래스입니다.
    /// </summary>
    public sealed class AIPlayerSpawnController : MonoBehaviourBase
    {
        private AIPlayerDataCurrency _currency;
        private AIPlayerDataUnit _unit;

        public void Init(AIPlayerRoot root)
        {
            AIPlayerDataModel dataModel = root.dataModel;
            AssertHelper.NotNull(typeof(AIPlayerSpawnController), dataModel);
            
            _currency = dataModel.Currency;
            _unit = dataModel.Unit;
        }

        /// <summary>
        /// AI 플레이어가 유닛을 생산할 수 있는지 확인합니다.
        /// </summary>
        /// <returns>유닛 생산이 가능하면 true, 불가능하면 false 반환</returns>
        public bool CanSpawnUnit()
        {
            // TODO: 현재 보유한 돈이 유닛 소환 비용보다 많은지 확인하는 로직 추가
            // TODO: 최대 소환 가능한 유닛 수를 초과하지 않는지 확인하는 로직 추가
            return false;
        }

        /// <summary>
        /// AI 플레이어가 유닛을 생산하는 동작을 수행합니다.
        /// </summary>
        /// <returns>생산 성공 여부</returns>
        public TaskStatus TrySpawnUnit()
        {
            // TODO: 유닛 생산을 시도하는 로직 추가
            // TODO: 성공적으로 소환되면 TaskStatus.Success 반환, 실패 시 TaskStatus.Failure 반환
            return TaskStatus.Failure;
        }
    }
}
