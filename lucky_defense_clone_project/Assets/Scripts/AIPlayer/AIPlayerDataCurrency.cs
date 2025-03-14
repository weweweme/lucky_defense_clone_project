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
        private void AddGold(uint amount) => _gold += amount;
        public void SubtractGold(uint amount) => _gold -= amount;
        
        // 다이아몬드 관련 데이터
        private const uint INITIAL_DIAMOND = 3;
        private uint _diamond = INITIAL_DIAMOND;
        public uint GetDiamond() => _diamond;
        public void AddDiamond(uint amount) => _diamond += amount;
        public void SubDiamond(uint amount) => _diamond -= amount;
        
        public AIPlayerDataCurrency(DataManager dataManager, CompositeDisposable disposable)
        {
            MDL_Enemy enemy = dataManager.Enemy;
            AssertHelper.NotNull(typeof(AIPlayerDataModel), enemy);
            enemy.OnEnemyDeath
                .Subscribe(_ => AddCurrencyOnEnemyDeath())
                .AddTo(disposable);
        } 
        
        private void AddCurrencyOnEnemyDeath()
        {
            AddGold(1);
            
            // 5% 확률로 다이아 지급
            if (UnityEngine.Random.value < 0.05f)  
            {
                AddDiamond(1);
            }
        }
    }
}
