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
        [SerializeField] internal Button _btnStart; 

        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_GameDoor), _gameDoor);
            AssertHelper.NotNull(typeof(VW_GameDoor), _mainTxt);
            AssertHelper.NotNull(typeof(VW_GameDoor), _subTxt);
            AssertHelper.NotNull(typeof(VW_GameDoor), _btnStart);
        }
        
        public async UniTaskVoid EndGameDirection()
        {
            _mainTxt.SetText("클리어 실패!");
            
            await CloseDoor();
            
            for (int i = 5; i >= 0; --i)
            {
                _subTxt.SetText("잠시 후 게임이 종료됩니다. ({0})", i);
                
                await UniTask.Delay(1000); // 1초
            }

            EditorApplication.isPlaying = false;
        }

#pragma warning disable CS1998 // 비동기 메서드에 'await'가 없지만, 설계상 문제없음.
        private async UniTask CloseDoor()
#pragma warning restore CS1998 
        {
            _gameDoor.DOAnchorPosY(0, 0.5f).SetEase(Ease.Linear);
        }
    }
}
