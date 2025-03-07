using System;
using System.Collections.Generic;
using InGame.System;
using Model;
using UniRx;
using Util;

namespace UI
{
    /// <summary>
    /// 도박 패널을 관리하는 Presenter 클래스입니다.
    /// </summary>
    public sealed class PR_GamblePanel : Presenter
    {
        private static readonly Dictionary<EUnitGrade, float> _gambleSuccessProbabilities = new()
        {
            { EUnitGrade.Rare, 0.6f },
            { EUnitGrade.Heroic, 0.2f },
            { EUnitGrade.Mythic, 0.1f }
        };
        private readonly MDL_Unit _mdlUnit;
        
        public override void Init(DataManager dataManager, View view)
        {
            AssertHelper.NotNull(typeof(PR_GamblePanel), dataManager);

            VW_GamblePanel vw = view as VW_GamblePanel;
            AssertHelper.NotNull(typeof(PR_GamblePanel), vw);
            
            MDL_Unit mdlUnit = dataManager.Unit;
            AssertHelper.NotNull(typeof(PR_GamblePanel), mdlUnit);

            MDL_GameSystem mdlSystem = dataManager.GameSystem;
            AssertHelper.NotNull(typeof(PR_MythicUnitCombinationPanel), mdlSystem);
            vw!.exitBackgroundPanel.OnClickAsObservable()
                .Subscribe(_ => mdlSystem.SetGamblePanelVisible(false))
                .AddTo(disposable);
            vw.exitButton.OnClickAsObservable()
                .Subscribe(_ => mdlSystem.SetGamblePanelVisible(false))
                .AddTo(disposable);
            mdlSystem.GamblePanelVisible
                .Subscribe(vw.SetGamblePanelVisible)
                .AddTo(disposable);
            
            foreach (var elem in vw.gambleTryItems)
            {
                elem.tryButton.OnClickAsObservable()
                    .Subscribe(_ => TryGamble(elem.unitGrade))
                    .AddTo(disposable);
            }
        }

        private void TryGamble(EUnitGrade grade)
        {
            float successProbability = GetGambleSuccessProbability(grade);
            bool isSuccess = UnityEngine.Random.Range(0f, 1f) < successProbability;

            if (!isSuccess)
            {
                // 실패 처리 로직 (UI 메시지 출력, 리소스 차감 등 추가 가능)
                UnityEngine.Debug.Log($"[Gamble Failed] Grade: {grade}");
                return;
            }

            // 성공 시 유닛 소환
            SUnitSpawnRequestData data = new SUnitSpawnRequestData(grade, GetRandomType(), EPlayerSide.South);
            _mdlUnit.SpawnUnit(data);

            UnityEngine.Debug.Log($"[Gamble Success] Grade: {grade}");
        }

        /// <summary>
        /// 등급별 도박 성공 확률을 반환합니다.
        /// </summary>
        /// <param name="grade">도박 대상 유닛 등급</param>
        /// <returns>성공 확률 (0.0f ~ 1.0f)</returns>
        private float GetGambleSuccessProbability(EUnitGrade grade)
        {
            AssertHelper.NotEqualsEnum(typeof(PR_GamblePanel), grade, EUnitGrade.None);
            
            if (_gambleSuccessProbabilities.TryGetValue(grade, out float probability))
            {
                return probability;
            }
            
            throw new ArgumentOutOfRangeException(nameof(grade), grade, "Unsupported grade for gambling.");
        }

        /// <summary>
        /// 근거리와 원거리를 60%/40%로 랜덤 선택합니다.
        /// </summary>
        /// <returns>랜덤 유닛 타입</returns>
        private EUnitType GetRandomType()
        {
            return UnityEngine.Random.Range(0f, 1f) < 0.6f ? EUnitType.Melee : EUnitType.Ranged;
        }
    }
}
