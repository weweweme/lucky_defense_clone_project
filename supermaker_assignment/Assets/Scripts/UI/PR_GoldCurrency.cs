using System;
using InGame.System;
using Model;
using Util;
using UniRx;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// 골드와 관련된 UI를 관리하는 Presenter 클래스입니다.
    /// </summary>
    public class PR_GoldCurrency : Presenter
    {
        private MDL_Currency _currency;
        
        public override void Init(DataManager dataManager, View view)
        {
            AssertHelper.NotNull(typeof(PR_GoldCurrency), dataManager);

            _currency = dataManager.Currency;
            AssertHelper.NotNull(typeof(PR_GoldCurrency), _currency);
            
            VW_Currency vw = view as VW_Currency;
            AssertHelper.NotNull(typeof(PR_GoldCurrency), vw);
            
            _currency.Gold
                .Subscribe(vw!.UpdateCurrency)
                .AddTo(disposable);
            
            MDL_Enemy enemy = dataManager.Enemy;
            AssertHelper.NotNull(typeof(PR_GoldCurrency), enemy);
            enemy.OnEnemyDeath
                .Subscribe(_ => RewardCurrencyOnEnemyDeath())
                .AddTo(disposable);
        }
        
        /// <summary>
        /// 적이 사망했을 때 플레이어에게 골드를 지급합니다.
        /// </summary>
        private void RewardCurrencyOnEnemyDeath()
        {
            // 기본 골드 지급
            _currency.AddGold(1);

            // 1% 확률로 다이아 지급
            if (UnityEngine.Random.value < 0.01f)  
            {
                _currency.AddDiamond(1);
            }
        }
    }
}
