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
        private VW_MythicUnitCombinationPanel _vw;
        private EUnitType _currentClickedUnitType;
        private MDL_GameSystem _mdlGameSystem;

        public override void Init(DataManager dataManager, View view)
        {
            AssertHelper.NotNull(typeof(PR_MythicUnitCombinationPanel), dataManager);

            _vw = view as VW_MythicUnitCombinationPanel;
            AssertHelper.NotNull(typeof(PR_MythicUnitCombinationPanel), _vw);

            _mdlGameSystem = dataManager.GameSystem;
            AssertHelper.NotNull(typeof(PR_MythicUnitCombinationPanel), _mdlGameSystem);
            _mdlGameSystem.MythicCombinationPanelVisible
                .Subscribe(_vw!.SetCanvasActive)
                .AddTo(disposable);
            _vw.exitBackgroundPanel.OnClickAsObservable()
                .Subscribe(_ => _mdlGameSystem.SetMythicCombinationPanelVisible(false))
                .AddTo(disposable);
            _vw.exitButton.OnClickAsObservable()
                .Subscribe(_ => _mdlGameSystem.SetMythicCombinationPanelVisible(false))
                .AddTo(disposable);
            _vw.combineBut.OnClickAsObservable()
                .Subscribe(_ => TryCombineMythicUnit())
                .AddTo(disposable);

            _mdlMythicUnitCombination = dataManager.MythicUnitCombination;
            _combinationCheckers = _mdlMythicUnitCombination.GetCombinationFlagCheckers();
            
            _mdlUnit = dataManager.Unit; 

            _mdlMythicUnitCombination.OnMythicUnitCombination
                .Subscribe(OnClickCombinationUnitListItem)
                .AddTo(disposable);
            
            _mdlGameSystem.MythicCombinationPanelVisible
                .Subscribe(isVisible =>
                {
                    if (isVisible)
                        AddAllSouthUnitsToCheckers(); // 모든 South 노드 추가
                    else
                        RemoveAllSouthUnitsFromCheckers(); // 모든 South 노드 제거
                })
                .AddTo(disposable);
        }

        private void TryCombineMythicUnit()
        {
            _removeNodes.Clear(); // 재사용 리스트 초기화

            // 조합 가능한 유닛 탐색
            UnitCombinationPossibleChecker selectedChecker = null;

            // 현재 클릭된 신화 유닛을 담당하는 Checker 탐색
            foreach (var elem in _combinationCheckers)
            {
                if (elem.ResultUnitType == _currentClickedUnitType)
                {
                    selectedChecker = elem;
                    break;
                }
            }
            
            AssertHelper.NotNull(typeof(PR_MythicUnitCombinationPanel), selectedChecker);

            // 합쳐지기 전, 제물이 되는 유닛 제거를 위한 노드 수집
            foreach (var nodeEntry in selectedChecker!.NodeConditionMap) // 이미 매칭된 노드 활용
            {
                _removeNodes.Add(nodeEntry.Key);
            }

            // 유닛 제거 수행
            foreach (var node in _removeNodes)
            {
                node.SubUnit(); // 기존 유닛 제거
            }

            ApplyUnitCount();

            // 신화 유닛 소환
            SUnitSpawnRequestData spawnData = new SUnitSpawnRequestData(
                EUnitGrade.Mythic,
                selectedChecker.ResultUnitType,
                EPlayerSide.South);

            _mdlUnit.SpawnUnit(spawnData);
            _mdlGameSystem.SetMythicCombinationPanelVisible(false);
        }
        
        /// <summary>
        /// 조합을 위한 유닛 수량을 적용하는 메서드입니다.
        /// </summary>
        private void ApplyUnitCount()
        {
            const uint SUB_COMBINATION_UNIT_COUNT = 2; // -3 + 1 = 2
            uint currentSpawnCount = _mdlUnit.GetCurrentSpawnCount();
            _mdlUnit.SetCurrentSpawnCount(currentSpawnCount - SUB_COMBINATION_UNIT_COUNT);
        }

        private void OnClickCombinationUnitListItem(SCurrentMythicUnitCombinationData data)
        {
            _vw.SetCurrentUnitData(data);
            _currentClickedUnitType = data.UnitType;
        }
        
        private void AddAllSouthUnitsToCheckers()
        {
            var southGridNodes = RootManager.Ins.UnitGridNodeManager.SouthGridNodes;

            foreach (var node in southGridNodes)
            {
                if (node.UnitGroup.IsEmpty()) continue;

                foreach (var checker in _combinationCheckers)
                {
                    checker.HandleAddUnit(node);
                }
            }
        }

        private void RemoveAllSouthUnitsFromCheckers()
        {
            var southGridNodes = RootManager.Ins.UnitGridNodeManager.SouthGridNodes;

            foreach (var node in southGridNodes)
            {
                if (node.UnitGroup.IsEmpty()) continue;

                foreach (var checker in _combinationCheckers)
                {
                    checker.HandleRemoveUnit(node);
                }
            }
        }
    }
}
