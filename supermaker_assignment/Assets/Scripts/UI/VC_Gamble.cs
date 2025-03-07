using System;
using UnityEngine;
using Util;

namespace UI
{
    /// <summary>
    /// 갬블 관련 UI들을 관리하는 ViewController 클래스입니다.
    /// </summary>
    public sealed class VC_Gamble : ViewController
    {
        [SerializeField] private VW_GambleButton _vwGambleButton;
        private readonly PR_GambleButton _prGambleButton = new PR_GambleButton();
        
        [SerializeField] private VW_GamblePanel _vwGamblePanel;
        private readonly PR_GamblePanel _prGamblePanel = new PR_GamblePanel();
        
        protected override void ValidateReferences()
        {
            AssertHelper.NotNull(typeof(VC_Gamble), _vwGambleButton);
            AssertHelper.NotNull(typeof(VC_Gamble), _vwGamblePanel);
        }

        public override void Init(DataManager dataManager)
        {
            _prGambleButton.Init(dataManager, _vwGambleButton);
            _prGamblePanel.Init(dataManager, _vwGamblePanel);
        }

        protected override void ReleasePresenter()
        {
            _prGambleButton.Dispose();
            _prGamblePanel.Dispose();
        }
    }
}
