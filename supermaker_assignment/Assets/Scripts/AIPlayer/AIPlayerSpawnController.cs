using Util;

namespace AIPlayer
{
    /// <summary>
    /// AI Player의 스폰과 관련된 로직을 관리하는 컨트롤러 클래스입니다.
    /// </summary>
    public sealed class AIPlayerSpawnController : MonoBehaviourBase
    {
        /// <summary>
        /// AI 플레이어의 루트 클래스입니다.
        /// </summary>
        private AIPlayerRoot _root;

        public void Init(AIPlayerRoot root)
        {
            _root = root;
        }
    }
}
