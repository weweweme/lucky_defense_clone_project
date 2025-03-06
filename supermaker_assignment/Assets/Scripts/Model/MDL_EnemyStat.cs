using UniRx;

namespace Model
{
    /// <summary>
    /// 적 유닛의 스탯과 관련된 데이터를 관리하는 모델 클래스입니다.
    /// </summary>
    public class MDL_EnemyStat
    {
        // 적의 체력과 관련된 Rx
        private readonly ReactiveProperty<uint> _hp = new ReactiveProperty<uint>();
        public IReactiveProperty<uint> Hp => _hp;
        
        // 적의 최대 체력
        private uint _maxHp;
        public uint MaxHp => _maxHp;
        
        public void SetStat(uint maxHp)
        {
            _maxHp = maxHp;
            _hp.Value = maxHp;
        }
    }
}
