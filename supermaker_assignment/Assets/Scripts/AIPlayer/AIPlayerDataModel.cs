namespace AIPlayer
{
    /// <summary>
    /// AI 플레이어의 데이터 모델 클래스입니다.
    /// </summary>
    public class AIPlayerDataModel
    {
        // 골드 관련 데이터
        private const uint INITIAL_GOLD = 100;
        private uint _gold = INITIAL_GOLD;
        public uint GetGold() => _gold;
        public void AddGold(uint amount) => _gold += amount;
        public void SubtractGold(uint amount) => _gold -= amount;
    }
}
