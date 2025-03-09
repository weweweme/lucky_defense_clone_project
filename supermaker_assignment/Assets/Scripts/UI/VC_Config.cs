using System;
using UnityEngine;
using Util;

namespace UI
{
    /// <summary>
    /// Config과 관련된 Presenter와 View를 연결하는 ViewController 클래스입니다.
    /// </summary>
    public sealed class VC_Config : ViewController
    {
        [SerializeField] private VW_GameSpeedMultiplier _vwGameSpeedMultiplier;
        private readonly PR_GameSpeedMultiplier _prGameSpeedMultiplier = new PR_GameSpeedMultiplier();
        
        [SerializeField] private VW_GameDoor _vwGameDoor;
        private readonly PR_GameDoor _prGameDoor = new PR_GameDoor();
        
        protected override void ValidateReferences()
        {
            AssertHelper.NotNull(typeof(VC_Config), _vwGameSpeedMultiplier);
            AssertHelper.NotNull(typeof(VC_Config), _vwGameDoor);
        }

        public override void Init(DataManager dataManager)
        {
            _prGameSpeedMultiplier.Init(dataManager, _vwGameSpeedMultiplier);
            _prGameDoor.Init(dataManager, _vwGameDoor);
        }

        protected override void ReleasePresenter()
        {
            _prGameSpeedMultiplier.Dispose();
            _prGameDoor.Dispose();
        }
    }
}
