using System;
using Model;
using UniRx;
using Util;

namespace AIPlayer
{
    /// <summary>
    /// AI 플레이어의 유닛 관련 데이터 클래스입니다.
    /// </summary>
    public class AIPlayerDataUnit
    {
        public AIPlayerDataUnit(DataManager dataManager, CompositeDisposable disposable)
        {
            MDL_Unit unit = dataManager.Unit;
            AssertHelper.NotNull(typeof(AIPlayerDataModel), unit);
        } 
    }
}
