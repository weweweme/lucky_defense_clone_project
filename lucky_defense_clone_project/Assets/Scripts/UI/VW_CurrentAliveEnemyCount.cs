using InGame.System;
using TMPro;
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
        [SerializeField] private TextMeshProUGUI _aliveEnemyCountText;
        private const float MAX_ENEMY_COUNT = 100f;
        [SerializeField] private RectTransform _fireEffect;

        // Scale 범위 캐싱
        private readonly Vector3 _minScale = new Vector3(0.5f, -0.7f, 1f);
        private readonly Vector3 _maxScale = new Vector3(2.2f, -3.3f, 1f);

        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_CurrentAliveEnemyCount), _aliveEnemyPercentImg);
            AssertHelper.NotNull(typeof(VW_CurrentAliveEnemyCount), _aliveEnemyCountText);
            AssertHelper.NotNull(typeof(VW_CurrentAliveEnemyCount), _fireEffect);
        }

        public void UpdateAliveEnemyUI(uint aliveEnemyCount)
        {
            _aliveEnemyCountText.SetText("{0} / {1}", aliveEnemyCount, MAX_ENEMY_COUNT);
        
            float normalizedValue = aliveEnemyCount / MAX_ENEMY_COUNT;

            // normalizedValue 값이 0과 1 사이에 있도록 보장
            normalizedValue = Mathf.Clamp01(normalizedValue);
            _aliveEnemyPercentImg.fillAmount = normalizedValue; 

            // _fireEffect의 Scale 보간
            _fireEffect.localScale = Vector3.Lerp(_minScale, _maxScale, normalizedValue);
        }
    }
}
