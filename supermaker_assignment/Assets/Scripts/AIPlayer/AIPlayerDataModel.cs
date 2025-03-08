using System;
using Model;
using UniRx;
using Util;

namespace AIPlayer
{
    /// <summary>
    /// AI 플레이어의 데이터 모델 클래스입니다.
    /// </summary>
    public sealed class AIPlayerDataModel : IDisposable
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        // 골드 관련 데이터
        private const uint INITIAL_GOLD = 100;
        private uint _gold = INITIAL_GOLD;
        public uint GetGold() => _gold;
        public void AddGold(uint amount) => _gold += amount;
        public void SubtractGold(uint amount) => _gold -= amount;

        public AIPlayerDataModel(DataManager dataManager)
        {
            MDL_Enemy enemy = dataManager.Enemy;
            AssertHelper.NotNull(typeof(AIPlayerDataModel), enemy);
            enemy.OnEnemyDeath
                .Subscribe(_ => _gold += 1)
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
