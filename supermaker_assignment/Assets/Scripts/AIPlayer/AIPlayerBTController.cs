using System.Threading;
using CleverCrow.Fluid.BTs.Trees;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Util;

namespace AIPlayer
{
    /// <summary>
    /// 상대 AI의 행동을 제어하는 BT 클래스입니다.
    /// </summary>
    public sealed class AIPlayerBTController : MonoBehaviourBase
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
        /// 유닛의 행동을 결정하는 트리입니다.
        /// </summary>
        private BehaviorTree _bt;
        
        /// <summary>
        /// AI 플레이어의 루트 클래스입니다.
        /// </summary>
        private AIPlayerRoot _root;
        
        /// <summary>
        /// AI 플레이어의 스폰 컨트롤러입니다.
        /// </summary>
        private AIPlayerSpawnController _spawnController;
        
        /// <summary>
        /// AI 플레이어의 도박 컨트롤러입니다.
        /// </summary>
        private AIPlayerGambleController _gambleController;
        
        /// <summary>
        /// AI 플레이어의 유닛 합성 컨트롤러입니다.
        /// </summary>
        private AIPlayerMergeController _mergeController;
        
        /// <summary>
        /// AI 플레이어의 신화 유닛 조합 컨트롤러입니다.
        /// </summary>
        private AIPlayerMythicUnitCombinationController _mythicUnitCombinationController;

        public void Init(AIPlayerRoot root)
        {
            _root = root;
            _spawnController = root.spawnController;
            _gambleController = root.gambleController;
            _mergeController = root.mergeController;
            _mythicUnitCombinationController = root.mythicUnitCombinationController;
            _bt = CreateTree(root.gameObject);
        }
        
        /// <summary>
        /// AI 상대 플레이어의 행동 트리를 구성하고 반환합니다.
        /// 자원 상태와 유닛 상태에 따라 행동을 결정합니다.
        /// </summary>
        /// <param name="owner">행동 트리의 소유 게임 오브젝트</param>
        /// <returns>구성된 BehaviorTree 인스턴스</returns>
        private BehaviorTree CreateTree(GameObject owner)
        {
            BehaviorTree bt = new BehaviorTreeBuilder(owner)
                .Selector("최우선 행동 선택")

                    // 1. 유닛 합성이 가능한 경우 합성
                    .Sequence("유닛 합성 가능할 때")
                        .Condition("합성 가능한 유닛이 있는가?", _mergeController.CanMerge)
                        .Do("유닛 합성 실행", () => _mergeController.TryMerge())
                    .End()

                    // 2. 도박이 가능한 경우 도박 진행
                    .Sequence("도박 가능할 때")
                        .Condition("도박에 필요한 돌이 있는가?", _gambleController.CanGamble)
                        .Do("도박 실행", () => _gambleController.TryGamble())
                    .End()

                    // 3. 유닛 생산 (돈이 있을 경우)
                    .Sequence("유닛 생산 가능할 때")
                        .Condition("유닛을 생산할 돈이 있는가?", _spawnController.CanSpawnUnit)
                        .Do("유닛 생산 실행", _spawnController.TrySpawnUnit)
                    .End()

                .End()
            .Build();

            return bt;
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
        /// 유닛이 제거되거나 비활성화될 때 호출되어,
        /// 행동 트리 실행을 중단하고 리소스를 정리합니다.
        /// </summary>
        public void Dispose()
        {
            CancelTokenHelper.DisposeToken(in _cts);
        }
    }
}
