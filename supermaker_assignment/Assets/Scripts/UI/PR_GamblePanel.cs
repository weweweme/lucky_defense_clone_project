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
        private static readonly Dictionary<EUnitGrade, SGambleMetaData> GAMBLE_META_DATA = new()
        {
            { EUnitGrade.Rare, new SGambleMetaData(EUnitGrade.Rare, 0.6f, 1) },
            { EUnitGrade.Heroic, new SGambleMetaData(EUnitGrade.Heroic, 0.2f, 1) },
            { EUnitGrade.Mythic, new SGambleMetaData(EUnitGrade.Mythic, 0.1f, 2) }
        };
        private MDL_Unit _mdlUnit;
        private MDL_Currency _mdlCurrency;
        
        public override void Init(DataManager dataManager, View view)
        {
            AssertHelper.NotNull(typeof(PR_GamblePanel), dataManager);

            VW_GamblePanel vw = view as VW_GamblePanel;
            AssertHelper.NotNull(typeof(PR_GamblePanel), vw);
            
            _mdlUnit = dataManager.Unit;
            AssertHelper.NotNull(typeof(PR_GamblePanel), _mdlUnit);
            _mdlCurrency = dataManager.Currency;
            AssertHelper.NotNull(typeof(PR_UnitSpawn), _mdlCurrency);

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
            if (!IsPossibleGamble(grade)) return;

            float successProbability = GetGambleSuccessProbability(grade);
            bool isSuccess = UnityEngine.Random.Range(0f, 1f) < successProbability;
            if (!isSuccess)
            {
                // TODO: 실패 UX 추가
                UnityEngine.Debug.Log($"[Gamble Failed] Grade: {grade}");
                return;
            }

            // TODO: 성공 UX 추가
            SUnitSpawnRequestData data = new SUnitSpawnRequestData(grade, GetRandomType(), EPlayerSide.South);
            _mdlUnit.SpawnUnit(data);

            UnityEngine.Debug.Log($"[Gamble Success] Grade: {grade}");
        }
        
        /// <summary>
        /// 해당 등급의 유닛 도박(Gamble)이 가능한지 여부를 판단합니다.
        /// 도박 가능 조건:
        /// 1. 유닛 소환이 가능한 상태인지 확인
        /// 2. 유닛 등급(Grade)이 유효한지 검증
        /// 3. 현재 보유 다이아(Diamond)가 요구량 이상인지 확인
        /// </summary>
        /// <param name="grade">도박하려는 유닛의 등급</param>
        /// <returns>
        /// 도박 가능 여부 (true: 가능, false: 불가능)
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">지원되지 않는 유닛 등급을 입력한 경우 발생</exception>
        private bool IsPossibleGamble(EUnitGrade grade)
        {
            if (!_mdlUnit.IsSpawnPossible()) return false;

            AssertHelper.NotEqualsEnum(typeof(PR_GamblePanel), grade, EUnitGrade.None);

            if (GAMBLE_META_DATA.TryGetValue(grade, out SGambleMetaData metaData))
            {
                uint currentAvailableDia = _mdlCurrency.GetDiamond();
                return currentAvailableDia >= metaData.RequiredDia;
            }
           
            throw new ArgumentOutOfRangeException(nameof(grade), grade, "Unsupported grade for gambling."); 
        }

        /// <summary>
        /// 등급별 도박 성공 확률을 반환합니다.
        /// </summary>
        /// <param name="grade">도박 대상 유닛 등급</param>
        /// <returns>성공 확률 (0.0f ~ 1.0f)</returns>
        private float GetGambleSuccessProbability(EUnitGrade grade)
        {
            AssertHelper.NotEqualsEnum(typeof(PR_GamblePanel), grade, EUnitGrade.None);
    
            if (GAMBLE_META_DATA.TryGetValue(grade, out SGambleMetaData metaData))
            {
                return metaData.SuccessProbability;
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
