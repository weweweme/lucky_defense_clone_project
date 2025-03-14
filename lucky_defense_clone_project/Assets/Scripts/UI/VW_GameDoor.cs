using Cysharp.Threading.Tasks;
using InGame.System;
using TMPro;
using UnityEngine;
using Util;
using DG.Tweening;
using UnityEditor;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// 게임 시작과 끝을 나타내는 View 클래스입니다.
    /// </summary>
    public sealed class VW_GameDoor : View
    {
        [SerializeField] private RectTransform _gameDoor;
        [SerializeField] private TextMeshProUGUI _mainTxt;
        [SerializeField] private TextMeshProUGUI _subTxt;
        [SerializeField] internal Button startBtn; 
        
        private bool _isBlinking = true; // 알파값 변경을 제어하는 플래그

        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_GameDoor), _gameDoor);
            AssertHelper.NotNull(typeof(VW_GameDoor), _mainTxt);
            AssertHelper.NotNull(typeof(VW_GameDoor), _subTxt);
            AssertHelper.NotNull(typeof(VW_GameDoor), startBtn);
            
            _subTxt.gameObject.SetActive(false);
        }

        private void Start()
        {
            BlinkMainText().Forget(); // 부드러운 깜빡임 시작
        }
        
        public async UniTaskVoid StartGameDirection()
        {
            _mainTxt.SetText("게임 시작!");
            
            await DoorAction(true);
            _isBlinking = false; // 깜빡임 정지
            _mainTxt.DOFade(1f, 0.5f); // 알파값 1로 고정
        }
        
        /// <summary>
        /// _mainTxt 알파값을 부드럽게 1초 간격으로 0과 1 사이로 전환하는 비동기 루프
        /// </summary>
        private async UniTaskVoid BlinkMainText()
        {
            while (_isBlinking)
            {
                await _mainTxt.DOFade(0f, 1f).SetEase(Ease.InOutSine).AsyncWaitForCompletion();
                await _mainTxt.DOFade(1f, 1f).SetEase(Ease.InOutSine).AsyncWaitForCompletion();
            }

            // 깜빡임이 멈춘 후 알파값 1로 설정
            _mainTxt.DOFade(1f, 0.5f);
        }

        /// <summary>
        /// 게임 종료 연출 (클리어/실패 구분 가능)
        /// </summary>
        /// <param name="isClear">true면 클리어 성공, false면 실패</param>
        public async UniTaskVoid GameEndDirection(bool isClear)
        {
            _mainTxt.SetText(isClear ? "클리어 성공!" : "클리어 실패!");
            
            await DoorAction(false);
            
            _subTxt.gameObject.SetActive(true);
            
            for (int i = 5; i >= 0; --i)
            {
                _subTxt.SetText($"잠시 후 게임이 종료됩니다. ({i})");
                
                await UniTask.Delay(1000); // 1초
            }

            EditorApplication.isPlaying = false;
        }

        /// <summary>
        /// 게임 문을 여닫는 애니메이션을 실행합니다.
        /// </summary>
        /// <param name="isOpen">true면 문을 열고, false면 닫습니다.</param>
#pragma warning disable CS1998 // 이 비동기 메서드에는 'await' 연산자가 없지만 설계상 문제 없음
        private async UniTask DoorAction(bool isOpen)
#pragma warning restore CS1998 // 이 비동기 메서드에는 'await' 연산자가 없지만 설계상 문제 없음
        {
            float targetY = isOpen ? 1920f : 0f;
            _gameDoor.DOAnchorPosY(targetY, 0.5f).SetEase(Ease.Linear);
        }
    }
}
