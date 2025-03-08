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
        private PR_GameSpeedMultiplier _prGameSpeedMultiplier = new PR_GameSpeedMultiplier();
        
        protected override void ValidateReferences()
        {
            AssertHelper.NotNull(typeof(VC_Config), _vwGameSpeedMultiplier);
        }

        public override void Init(DataManager dataManager)
        {
            _prGameSpeedMultiplier.Init(dataManager, _vwGameSpeedMultiplier);
        }

        protected override void ReleasePresenter()
        {
            _prGameSpeedMultiplier.Dispose();
        }
    }
}
