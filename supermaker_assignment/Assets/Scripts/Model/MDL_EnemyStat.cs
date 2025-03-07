using UniRx;

namespace Model
{
    /// <summary>
    /// 적 유닛의 스탯과 관련된 데이터를 관리하는 모델 클래스입니다.
    /// </summary>
    public class MDL_EnemyStat
    {
        // 적의 체력과 관련된 Rx
        private readonly ReactiveProperty<int> _hp = new ReactiveProperty<int>();
        public IReactiveProperty<int> Hp => _hp;

        // 적의 최대 체력
        private int _maxHp;
        public int MaxHp => _maxHp;

        /// <summary>
        /// 적 유닛의 최대 체력과 현재 체력을 초기화합니다.
        /// </summary>
        /// <param name="maxHp">적 유닛의 최대 체력</param>
        public void SetStat(uint maxHp)
        {
            _maxHp = (int)maxHp;
            _hp.Value = (int)maxHp;
        }

        /// <summary>
        /// 적 유닛의 현재 체력을 지정된 값으로 설정합니다.
        /// </summary>
        /// <param name="hp">설정할 현재 체력 값</param>
        public void SetCurrentHp(int hp)
        {
            _hp.Value = hp;
        }

        /// <summary>
        /// HP가 1 이상인지 확인합니다.
        /// </summary>
        /// <returns>체력이 1 이상이면 true, 아니면 false</returns>
        public bool HasHpRemaining()
        {
            return _hp.Value > 0;
        }
    }
}
