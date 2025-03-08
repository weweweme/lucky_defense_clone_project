using System;
using System.Collections.Generic;
using InGame.System;
using Model;
using UniRx;
using Unit;
using Util;

namespace UI
{
    /// <summary>
    /// 신화 조합 패널을 관리하는 Presenter 클래스입니다.
    /// </summary>
    public sealed class PR_MythicUnitCombinationPanel : Presenter
    {
        private MDL_MythicUnitCombination _mdlMythicUnitCombination;
        private IReadOnlyList<UnitCombinationPossibleChecker> _combinationCheckers;
        private readonly List<UnitPlacementNode> _removeNodes = new List<UnitPlacementNode>();
        private MDL_Unit _mdlUnit;

        public override void Init(DataManager dataManager, View view)
        {
            AssertHelper.NotNull(typeof(PR_MythicUnitCombinationPanel), dataManager);

            VW_MythicUnitCombinationPanel vw = view as VW_MythicUnitCombinationPanel;
            AssertHelper.NotNull(typeof(PR_MythicUnitCombinationPanel), vw);

            MDL_GameSystem mdlGameSystem = dataManager.GameSystem;
            AssertHelper.NotNull(typeof(PR_MythicUnitCombinationPanel), mdlGameSystem);
            mdlGameSystem.MythicCombinationPanelVisible
                .Subscribe(vw!.SetCanvasActive)
                .AddTo(disposable);
            vw.exitBackgroundPanel.OnClickAsObservable()
                .Subscribe(_ => mdlGameSystem.SetMythicCombinationPanelVisible(false))
                .AddTo(disposable);
            vw.exitButton.OnClickAsObservable()
                .Subscribe(_ => mdlGameSystem.SetMythicCombinationPanelVisible(false))
                .AddTo(disposable);
            vw.combineBut.OnClickAsObservable()
                .Subscribe(_ => TryCombineMythicUnit())
                .AddTo(disposable);

            _mdlMythicUnitCombination = dataManager.MythicUnitCombination;
            _combinationCheckers = _mdlMythicUnitCombination.GetCombinationFlagCheckers();
            
            _mdlUnit = dataManager.Unit; 

            _mdlMythicUnitCombination.OnMythicUnitCombination
                .Subscribe(vw.SetCurrentUnitData)
                .AddTo(disposable);
        }

        private void TryCombineMythicUnit()
        {
            _removeNodes.Clear(); // 재사용 리스트 초기화

            // 조합 가능한 유닛 탐색
            UnitCombinationPossibleChecker selectedChecker = null;

            // TODO: 현재 클릭한 신화 유닛이 어떤 타입인지 확인하고 selectedChecker를 할당하는 기능 구현
            
            AssertHelper.NotNull(typeof(PR_MythicUnitCombinationPanel), selectedChecker);

            // 기존 유닛 제거를 위한 노드 수집
            foreach (var nodeEntry in selectedChecker!.NodeConditionMap) // 이미 매칭된 노드 활용
            {
                _removeNodes.Add(nodeEntry.Key);
            }

            // 유닛 제거 수행
            foreach (var node in _removeNodes)
            {
                node.SubUnit(); // 기존 유닛 제거
                selectedChecker.HandleRemoveUnit(node); // 내부 카운트 정리
            }

            // 신화 유닛 소환
            SUnitSpawnRequestData spawnData = new SUnitSpawnRequestData(
                EUnitGrade.Mythic,
                selectedChecker.ResultUnitType,
                EPlayerSide.North);

            _mdlUnit.SpawnUnit(spawnData);
        }
    }
}
