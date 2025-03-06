using UnityEngine;
using UnityEngine.InputSystem;
using Util;

namespace System
{
    /// <summary>
    /// 유닛 배치 노드 드래그 시스템에서 Input System 이벤트를 처리하는 클래스입니다.
    /// 마우스 위치 및 클릭 이벤트를 감지하여 외부로 알리는 역할을 수행합니다.
    /// </summary>
    public sealed class UnitPlacementNodeInputHandler : IDisposable, UnitPlacementNodeInputActions.IMousePositionActions
    {
        private readonly UnitPlacementNodeInputActions _inputActions;

        /// <summary>
        /// 현재 마우스 위치의 월드 좌표를 저장합니다.
        /// </summary>
        public Vector2 CurrentMouseWorldPosition { get; private set; }

        /// <summary>
        /// 마우스 좌클릭이 시작될 때 발생하는 이벤트입니다.
        /// </summary>
        public event Action OnLeftClickStarted;

        /// <summary>
        /// 마우스 좌클릭이 해제될 때 발생하는 이벤트입니다.
        /// </summary>
        public event Action OnLeftClickCanceled;

        private readonly Camera _mainCam;
        private readonly float _zDepth;

        /// <summary>
        /// Input 핸들러를 초기화합니다.
        /// </summary>
        /// <param name="mainCam">마우스 위치 변환에 사용할 메인 카메라</param>
        public UnitPlacementNodeInputHandler(Camera mainCam)
        {
            _mainCam = mainCam;
            AssertHelper.NotNull(typeof(UnitPlacementNodeInputHandler), _mainCam);
            
            _zDepth = -_mainCam.transform.position.z;

            _inputActions = new UnitPlacementNodeInputActions();
            _inputActions.MousePosition.SetCallbacks(this);
            _inputActions.Enable();
        }

        /// <summary>
        /// 사용이 끝난 Input 핸들러의 리소스를 정리합니다.
        /// </summary>
        public void Dispose()
        {
            _inputActions?.Dispose();
        }

        /// <summary>
        /// 마우스 위치 변경 시 호출되는 Input System 콜백입니다.
        /// 스크린 좌표를 월드 좌표로 변환해 저장합니다.
        /// </summary>
        /// <param name="context">Input System 이벤트 컨텍스트</param>
        public void OnMousePosition(InputAction.CallbackContext context)
        {
            Vector2 screenPos = context.ReadValue<Vector2>();
            Vector3 worldPos = _mainCam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, _zDepth));
            CurrentMouseWorldPosition = worldPos;
        }

        /// <summary>
        /// 마우스 좌클릭 상태 변경 시 호출되는 Input System 콜백입니다.
        /// 클릭 시작 및 해제 이벤트를 외부로 전달합니다.
        /// </summary>
        /// <param name="context">Input System 이벤트 컨텍스트</param>
        public void OnMouseLeftClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                OnLeftClickStarted?.Invoke();
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                OnLeftClickCanceled?.Invoke();
            }
        }
    }
}
