using System;
using System.Threading;
using CleverCrow.Fluid.BTs.Trees;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Util;

namespace Unit
{
    /// <summary>
    /// 유닛의 AI 로직을 관리하는 행동 트리(Behavior Tree) 컨트롤러 클래스입니다.
    /// </summary>
    public sealed class UnitBTController : IDisposable
    {
        /// <summary>
        /// 행동 트리 틱(Tick)의 실행 간격 (밀리초 단위)입니다.
        /// 유닛의 AI 판단 주기를 설정합니다.
        /// </summary>
        private const int TICK_INTERVAL = 50;
                
        /// <summary>
        /// 행동 트리 실행을 제어하는 취소 토큰 소스입니다.
        /// 행동 트리 실행 도중 유닛이 제거되거나 비활성화되면 취소 처리를 담당합니다.
        /// </summary>
        private CancellationTokenSource _cts;
        
        /// <summary>
        /// 유닛의 공격 로직을 담당하는 컨트롤러입니다.
        /// </summary>
        private readonly UnitAttackController _attackController;
        
        /// <summary>
        /// 유닛의 행동을 결정하는 트리입니다.
        /// </summary>
        private readonly BehaviorTree _bt;

        /// <summary>
        /// 유닛의 행동 트리 컨트롤러를 생성합니다.
        /// </summary>
        /// <param name="root">해당 유닛의 루트 오브젝트</param>
        public UnitBTController(UnitRoot root)
        {
            _attackController = root.attackController;
            _bt = CreateTree(root.gameObject);
        }
        
        /// <summary>
        /// 행동 트리의 비동기 틱(Tick) 실행을 시작합니다.
        /// 유닛이 활성화되면 호출되어야 합니다.
        /// </summary>
        public void StartBtTick()
        {
            CancelTokenHelper.GetToken(ref _cts);
            TickBtAsync(_cts.Token).Forget();
        }
        
        /// <summary>
        /// 특정 주기마다 행동 트리를 실행하는 비동기 루프입니다.
        /// 유닛의 행동을 주기적으로 갱신하는 역할을 합니다.
        /// </summary>
        /// <param name="token">작업 취소를 위한 CancellationToken</param>
        private async UniTask TickBtAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await UniTask.Delay(TICK_INTERVAL, cancellationToken: token);

                if (token.IsCancellationRequested)
                {
                    break;
                }

                _bt.Tick();
            }
        }
        
        /// <summary>
        /// 유닛의 행동 트리를 구성하고 반환합니다.
        /// 유닛의 상태 및 주변 상황에 따라 행동을 결정하는 로직을 정의합니다.
        /// </summary>
        /// <param name="owner">행동 트리의 소유 게임 오브젝트</param>
        /// <returns>구성된 BehaviorTree 인스턴스</returns>
        private BehaviorTree CreateTree(GameObject owner)
        {
            BehaviorTree bt = new BehaviorTreeBuilder(owner)
                .Selector("한 가지 행동 선택")
                
                    // 1. 현재 공격 중인지 확인
                    .Sequence("공격 중일 때")
                        .Condition("현재 공격 중인가?", _attackController.HasTarget)
                        .Selector("타겟 유지 or 해제")
                            .Condition("타겟이 여전히 사거리 안에 있는가?", _attackController.IsTargetInRange)
                            .Do("타겟 해제", _attackController.ClearTarget)
                        .End()
                    .End()
                
                    // 2. 타겟이 없으면 새 적 탐색
                    .Do("적 탐색", _attackController.FindTarget)
                
                .End()
            .Build();
            
            return bt;
        }
        
        /// <summary>
        /// 유닛이 제거되거나 비활성화될 때 호출되어,
        /// 행동 트리 실행을 중단하고 리소스를 정리합니다.
        /// </summary>
        public void Dispose()
        {
            CancelTokenHelper.DisposeToken(in _cts);
        }
    }
}
