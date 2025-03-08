using CleverCrow.Fluid.BTs.Tasks;
using Util;

namespace AIPlayer
{
    /// <summary>
    /// AI Player의 도박과 관련된 로직을 관리하는 컨트롤러 클래스입니다.
    /// </summary>
    public sealed class AIPlayerGambleController : MonoBehaviourBase
    {
        /// <summary>
        /// AI 플레이어의 루트 클래스입니다.
        /// </summary>
        private AIPlayerRoot _root;

        /// <summary>
        /// AI의 재화 상태를 관리하는 컨트롤러입니다.
        /// </summary>
        private AIPlayerDataCurrency _currency;

        public void Init(AIPlayerRoot root)
        {
            _root = root;

            // TODO: 도박 관련 초기화 로직 추가 (필요 시)
        }

        /// <summary>
        /// 현재 AI가 도박을 시도할 수 있는지 판단합니다.
        /// </summary>
        /// <returns>도박이 가능하면 true, 불가능하면 false 반환</returns>
        public bool CanGamble()
        {
            // TODO: 현재 AI가 도박을 진행할 수 있는 조건을 검토
            // 1. 필요한 다이아몬드(도박 비용)를 보유하고 있는가?
            // 2. 도박이 가능한 게임 상태인가?
            // 3. 도박 시스템이 활성화된 상태인가?

            return false; // 실제 로직을 구현 후 조건에 맞게 변경
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
