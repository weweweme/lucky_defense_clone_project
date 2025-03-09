using DG.Tweening;
using InGame.System;
using TMPro;
using UnityEngine;
using Util;
using Cysharp.Threading.Tasks;

namespace UI
{
    /// <summary>
    /// 웨이브 초기 설정을 보여주는 View 클래스입니다.
    /// </summary>
    public sealed class VW_InitializeWave : View
    {
        [SerializeField] private RectTransform _topWaveArrow;
        [SerializeField] private TextMeshProUGUI _topWaveCountTxt;

        [SerializeField] private RectTransform _bottomWaveArrow;
        [SerializeField] private TextMeshProUGUI _bottomWaveCountTxt;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_InitializeWave), _topWaveArrow);
            AssertHelper.NotNull(typeof(VW_InitializeWave), _topWaveCountTxt);
            AssertHelper.NotNull(typeof(VW_InitializeWave), _bottomWaveArrow);
            AssertHelper.NotNull(typeof(VW_InitializeWave), _bottomWaveCountTxt);
        }

        /// <summary>
        /// 5초 동안 화살표 흔들기 + 카운트다운 애니메이션 실행
        /// </summary>
        public async UniTaskVoid StartWaveCountdown()
        {
            const int DURATION = 5;
            const float SHAKE_AMOUNT = 2f;
            const float SHAKE_SPEED = 0.5f;
            
            // 흔들기 애니메이션
            _topWaveArrow.DOAnchorPosY(_topWaveArrow.anchoredPosition.y - SHAKE_AMOUNT, SHAKE_SPEED)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);

            _bottomWaveArrow.DOAnchorPosY(_bottomWaveArrow.anchoredPosition.y + SHAKE_AMOUNT, SHAKE_SPEED)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
            
            // 카운트다운 진행
            for (int i = DURATION; i >= 0; --i)
            {
                _topWaveCountTxt.SetText("{0}", i);
                _bottomWaveCountTxt.SetText("{0}", i);
                
                if (i == 0)
                {
                    // 0이 되면 텍스트 사라짐
                    _topWaveCountTxt.gameObject.SetActive(false);
                    _bottomWaveCountTxt.gameObject.SetActive(false);
                }

                await UniTask.Delay(1000);
            }

            // 5초 후 애니메이션 정지
            _topWaveArrow.DOKill();
            _bottomWaveArrow.DOKill();
            
            _topWaveArrow.gameObject.SetActive(false);
            _bottomWaveArrow.gameObject.SetActive(false);
            _topWaveCountTxt.gameObject.SetActive(false);
            _bottomWaveCountTxt.gameObject.SetActive(false);
        }
    }
}
