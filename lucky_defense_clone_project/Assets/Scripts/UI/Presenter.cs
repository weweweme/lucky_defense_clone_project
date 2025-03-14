using System;
using InGame.System;
using UniRx;

namespace UI
{
    /// <summary>
    /// 프로젝트의 MVP UI 구조에서 사용되는 Presenter 추상 클래스입니다.
    /// 각 UI Presenter는 이 클래스를 상속받아 구체적인 UI 로직을 구현합니다.
    /// </summary>
    public abstract class Presenter
    {
        /// <summary>
        /// Presenter의 초기화를 수행하는 추상 메서드입니다.
        /// DataManager를 통하여 필요한 모델에 접근합니다.
        /// 해당 Presenter가 관리할 View를 전달받아 초기 설정을 진행합니다.
        /// </summary>
        /// <param name="view">초기화할 View 객체</param>
        public abstract void Init(DataManager dataManager, View view, CompositeDisposable disposable);
    }
}
