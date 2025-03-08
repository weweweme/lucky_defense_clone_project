using InGame.System;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    /// <summary>
    /// 현재 남은 적의 수를 보여주는 View 클래스입니다.
    /// </summary>
    public sealed class VW_CurrentAliveEnemyCount : View
    {
        [SerializeField] private Image _aliveEnemyPercentImg;
        private const float MAX_ENEMY_COUNT = 100f;
        
        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_CurrentAliveEnemyCount), _aliveEnemyPercentImg);
        }

        public void UpdateRemainingUI(uint remainingEnemyCount)
        {
            float normalizedHealth = remainingEnemyCount / MAX_ENEMY_COUNT;
            
            // normalizedHealth 값이 0과 1 사이에 있는지 확인
            normalizedHealth = Mathf.Clamp01(normalizedHealth);
            _aliveEnemyPercentImg.fillAmount = normalizedHealth;   
        }
    }
}
