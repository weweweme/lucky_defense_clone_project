using System;
using InGame.System;
using Model;
using UniRx;
using Util;

namespace UI
{
    /// <summary>
    /// 게임 속도를 조절하는 UI의 Presenter 클래스입니다
    /// </summary>
    public sealed class PR_GameSpeedMultiplier : Presenter
    {
        private MDL_GameSystem _mdl;
        private VW_GameSpeedMultiplier _vw;
        
        public override void Init(DataManager dataManager, View view, CompositeDisposable disposable)
        {
            AssertHelper.NotNull(typeof(PR_GameSpeedMultiplier), dataManager);
            
            _mdl = dataManager.GameSystem;
            AssertHelper.NotNull(typeof(PR_GameSpeedMultiplier), _mdl);
            
            _vw = view as VW_GameSpeedMultiplier;
            AssertHelper.NotNull(typeof(PR_GameSpeedMultiplier), _vw);
            _vw!.speedUpBut.OnClickAsObservable()
                .Subscribe(_ => ToggleGameSpeed())
                .AddTo(disposable);
            _mdl.IsDoubleSpeed
                .Subscribe(ApplySpeed)
                .AddTo(disposable);
        }
        
        /// <summary>
        /// 현재 게임 속도를 반대 값으로 변경 (1배속 ↔ 2배속)
        /// </summary>
        private void ToggleGameSpeed()
        {
            bool newSpeedState = !_mdl.IsDoubleSpeedActive(); // 현재 값의 반대
            _mdl.SetDoubleGameSpeed(newSpeedState); // 적용
        }

        /// <summary>
        /// 현재 게임 속도를 표시하는 텍스트를 변경합니다.
        /// </summary>
        private void ApplySpeed(bool value)
        {
            _vw.SetMultiple(value ? "x2" : "x1");
            TimeHelper.SetTimeScale(value ? 2f : 1f);
        }
    }
}
