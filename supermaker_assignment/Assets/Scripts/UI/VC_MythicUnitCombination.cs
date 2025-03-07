using System;
using UnityEngine;
using Util;

namespace UI
{
    /// <summary>
    /// 신화 조합 관련 UI들을 관리하는 ViewController 클래스입니다.
    /// </summary>
    public sealed class VC_MythicUnitCombination : ViewController
    {
        [SerializeField] private VW_MythicUnitCombinationButton _vwMythicUnitCombinationButton;
        private readonly PR_MythicUnitCombinationButton _prMythicUnitCombinationButton = new PR_MythicUnitCombinationButton();
        
        [SerializeField] private VW_MythicUnitCombinationPanel _vwMythicUnitCombinationPanel;
        private readonly PR_MythicUnitCombinationPanel _prMythicUnitCombinationPanel = new PR_MythicUnitCombinationPanel();
        
        protected override void ValidateReferences()
        {
            AssertHelper.NotNull(typeof(VC_MythicUnitCombination), _vwMythicUnitCombinationButton);
            AssertHelper.NotNull(typeof(VC_MythicUnitCombination), _vwMythicUnitCombinationPanel);
        }

        public override void Init(DataManager dataManager)
        {
            _prMythicUnitCombinationButton.Init(dataManager, _vwMythicUnitCombinationButton);
            _prMythicUnitCombinationPanel.Init(dataManager, _vwMythicUnitCombinationPanel);
        }

        protected override void ReleasePresenter()
        {
            _prMythicUnitCombinationButton.Dispose();
            _prMythicUnitCombinationPanel.Dispose();
        }
    }
}
