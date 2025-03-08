using System;
using UniRx;

namespace AIPlayer
{
    /// <summary>
    /// AI 플레이어의 데이터 모델 클래스입니다.
    /// </summary>
    public sealed class AIPlayerDataModel : IDisposable
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private readonly AIPlayerDataCurrency _currency;
        public AIPlayerDataCurrency Currency => _currency;
        private readonly AIPlayerDataUnit _unit;
        public AIPlayerDataUnit Unit => _unit;
        
        public AIPlayerDataModel(DataManager dataManager)
        {
            _currency = new AIPlayerDataCurrency(dataManager, _disposable);
            _unit = new AIPlayerDataUnit(dataManager, _disposable);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
