using System;
using Model;
using UniRx;
using Util;

namespace AIPlayer
{
    /// <summary>
    /// AI 플레이어의 재화 관련 데이터 클래스입니다.
    /// </summary>
    public class AIPlayerDataCurrency
    {
        // 골드 관련 데이터
        private const uint INITIAL_GOLD = 100;
        private uint _gold = INITIAL_GOLD;
        public uint GetGold() => _gold;
        public void AddGold(uint amount) => _gold += amount;
        public void SubtractGold(uint amount) => _gold -= amount;
        
        public AIPlayerDataCurrency(DataManager dataManager, CompositeDisposable disposable)
        {
            MDL_Enemy enemy = dataManager.Enemy;
            AssertHelper.NotNull(typeof(AIPlayerDataModel), enemy);
            enemy.OnEnemyDeath
                .Subscribe(_ => _gold += 1)
                .AddTo(disposable);
        } 
    }
}
