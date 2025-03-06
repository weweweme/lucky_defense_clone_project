using UniRx;

namespace Model
{
    /// <summary>
    /// 재화와 관련된 데이터를 관리하는 모델 클래스.
    /// </summary>
    public class MDL_Currency
    {
        private const uint INITIAL_GOLD = 100;
        private readonly ReactiveProperty<uint> _gold = new ReactiveProperty<uint>(INITIAL_GOLD);
        public IReadOnlyReactiveProperty<uint> Gold => _gold;

        public uint GetGold() => _gold.Value;
        public void AddGold(uint amount) => _gold.Value += amount;
        public void SubtractGold(uint amount) => _gold.Value -= amount;
    }
}
