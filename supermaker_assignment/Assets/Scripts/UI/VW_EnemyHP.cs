using InGame.System;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    /// <summary>
    /// Enemy의 체력 게이지 관련 처리를 담당하는 뷰 클래스입니다.
    /// </summary>
    public class VW_EnemyHP : View
    {
        [SerializeField] private Canvas _canvas;

        /// <summary>
        /// 체력 바를 표시할 Image 컴포넌트입니다.
        /// </summary>
        [SerializeField] private Image _healthBarImage;
        
        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_EnemyHP), _canvas);
            AssertHelper.NotNull(typeof(VW_EnemyHP), _healthBarImage);
        }

        private void Start()
        {
            _canvas.worldCamera = Camera.main;
            _canvas.sortingLayerName = "Enemy";
        }
        
        /// <summary>
        /// 외부에서 전달된 노멀라이즈된 값을 사용하여 체력 UI를 업데이트합니다.
        /// </summary>
        /// <param name="normalizedHealth">0과 1 사이의 노멀라이즈된 체력 값</param>
        public void UpdateHealthUI(float normalizedHealth)
        {
            bool isHealthBarActive = normalizedHealth < 1;
            _canvas.enabled = isHealthBarActive;
            
            // normalizedHealth 값이 0과 1 사이에 있는지 확인
            normalizedHealth = Mathf.Clamp01(normalizedHealth);
            _healthBarImage.fillAmount = normalizedHealth;
        }
    }
}