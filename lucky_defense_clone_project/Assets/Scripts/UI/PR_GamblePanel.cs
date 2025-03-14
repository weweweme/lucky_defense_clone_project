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
        public struct SGambleResult
        {
            public EUnitGrade Grade;
            public bool IsSuccess;
        }
        
        // TODO: 도박 데이터 중복 제거
        private static readonly Dictionary<EUnitGrade, SGambleMetaData> GAMBLE_META_DATA = new()
        {
            { EUnitGrade.Rare, new SGambleMetaData(EUnitGrade.Rare, 0.6f, 1) },
            { EUnitGrade.Hero, new SGambleMetaData(EUnitGrade.Hero, 0.2f, 1) },
            { EUnitGrade.Mythic, new SGambleMetaData(EUnitGrade.Mythic, 0.1f, 2) }
        };
        private MDL_Unit _mdlUnit;
        private MDL_Currency _mdlCurrency;
        private readonly Subject<SGambleResult> _gambleResultSubject = new Subject<SGambleResult>();

        public override void Init(DataManager dataManager, View view, CompositeDisposable disposable)
        {
            AssertHelper.NotNull(typeof(PR_GamblePanel), dataManager);

            VW_GamblePanel vw = view as VW_GamblePanel;
            AssertHelper.NotNull(typeof(PR_GamblePanel), vw);
            vw!.SetGambleDescription(GAMBLE_META_DATA);
            
            _mdlUnit = dataManager.Unit;
            AssertHelper.NotNull(typeof(PR_GamblePanel), _mdlUnit);
            _mdlCurrency = dataManager.Currency;
            AssertHelper.NotNull(typeof(PR_GamblePanel), _mdlCurrency);

            MDL_GameSystem mdlSystem = dataManager.GameSystem;
            AssertHelper.NotNull(typeof(PR_GamblePanel), mdlSystem);
            vw.exitBackgroundPanel.OnClickAsObservable()
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
                elem.Value.tryButton.OnClickAsObservable()
                    .Subscribe(_ => TryGamble(elem.Value.unitGrade))
                    .AddTo(disposable);
            }
            
            _gambleResultSubject
                .Subscribe(vw.ShowGambleResult)
                .AddTo(disposable);
        }

        private void TryGamble(EUnitGrade grade)
        {
            AssertHelper.NotEqualsEnum(typeof(PR_GamblePanel), grade, EUnitGrade.None);
            AssertHelper.NotEqualsEnum(typeof(PR_GamblePanel), grade, EUnitGrade.Common);

            if (!_mdlUnit.HasValidNodes) return;
            if (!IsPossibleGamble(grade)) return;

            ConsumeGambleCost(grade);
            
            float successProbability = GetGambleSuccessProbability(grade);
            bool isSuccess = UnityEngine.Random.Range(0f, 1f) < successProbability;
            if (!isSuccess)
            {
                _gambleResultSubject.OnNext(new SGambleResult { Grade = grade, IsSuccess = false });
                return;
            }

            _gambleResultSubject.OnNext(new SGambleResult { Grade = grade, IsSuccess = true });
            SUnitSpawnRequestData data = new SUnitSpawnRequestData(grade, GetRandomType(), EPlayerSide.South);
            _mdlUnit.SpawnUnit(data);
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
            if (!_mdlUnit.HasValidNodes) return false;

            AssertHelper.NotEqualsEnum(typeof(PR_GamblePanel), grade, EUnitGrade.None);
            AssertHelper.NotEqualsEnum(typeof(PR_GamblePanel), grade, EUnitGrade.Common);

            if (GAMBLE_META_DATA.TryGetValue(grade, out SGambleMetaData metaData))
            {
                uint currentAvailableDia = _mdlCurrency.GetDiamond();
                return currentAvailableDia >= metaData.RequiredDia;
            }
           
            throw new ArgumentOutOfRangeException(nameof(grade), grade, "Unsupported grade for gambling."); 
        }
        
        /// <summary>
        /// 도박 비용을 차감합니다.
        /// </summary>
        /// <param name="grade">어떤 등급 도박을 수행했는지 알 수 있는 등급</param>
        private void ConsumeGambleCost(EUnitGrade grade)
        {
            AssertHelper.NotEqualsEnum(typeof(PR_GamblePanel), grade, EUnitGrade.None);
            AssertHelper.NotEqualsEnum(typeof(PR_GamblePanel), grade, EUnitGrade.Common);

            if (GAMBLE_META_DATA.TryGetValue(grade, out SGambleMetaData metaData))
            {
                _mdlCurrency.SubDiamond(metaData.RequiredDia);
            }
        }

        /// <summary>
        /// 등급별 도박 성공 확률을 반환합니다.
        /// </summary>
        /// <param name="grade">도박 대상 유닛 등급</param>
        /// <returns>성공 확률 (0.0f ~ 1.0f)</returns>
        private float GetGambleSuccessProbability(EUnitGrade grade)
        {
            AssertHelper.NotEqualsEnum(typeof(PR_GamblePanel), grade, EUnitGrade.None);
            AssertHelper.NotEqualsEnum(typeof(PR_GamblePanel), grade, EUnitGrade.Common);
    
            if (GAMBLE_META_DATA.TryGetValue(grade, out SGambleMetaData metaData))
            {
                return metaData.SuccessProbability;
            }
    
            throw new ArgumentOutOfRangeException(nameof(grade), grade, "Unsupported grade for gambling.");
        }

        /// <summary>
        /// 근거리와 원거리를 랜덤 선택합니다.
        /// </summary>
        /// <returns>랜덤 유닛 타입</returns>
        private EUnitType GetRandomType()
        {
            float meleeRatio = 0.5f;
            
            return UnityEngine.Random.Range(0f, 1f) < meleeRatio ? EUnitType.Melee : EUnitType.Ranged;
        }
    }
}
