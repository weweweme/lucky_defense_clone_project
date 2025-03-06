using UniRx;

namespace Model
{
    /// <summary>
    /// 재화와 관련된 데이터를 관리하는 모델 클래스.
    /// </summary>
    public class MDL_Currency
    {
        private readonly ReactiveProperty<uint> _gold = new ReactiveProperty<uint>(0);
        public IReadOnlyReactiveProperty<uint> Gold => _gold;

        public void AddGold(uint amount) => _gold.Value += amount;
        public void SubtractGold(uint amount) => _gold.Value -= amount;
    }
}
