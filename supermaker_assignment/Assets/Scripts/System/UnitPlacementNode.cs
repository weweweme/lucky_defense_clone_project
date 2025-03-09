using Cysharp.Threading.Tasks;
using Unit;
using UnityEngine;
using Util;

namespace System
{
    /// <summary>
    /// 유닛이 배치되는 단일 노드를 나타내는 클래스입니다.
    /// 유닛의 등급, 타입, 개수 및 다인 배치 시 위치 정보를 관리합니다.
    /// </summary>
    public sealed class UnitPlacementNode : MonoBehaviourBase
    {
        [SerializeField] private uint _nodeIndex = uint.MaxValue;
        public uint NodeIndex => _nodeIndex;
        
        /// <summary>
        /// 해당 노드가 관리하는 유닛 그룹입니다.
        /// </summary>
        public UnitGroup UnitGroup = new UnitGroup();

        /// <summary>
        /// 1개 유닛 배치 시 사용되는 위치입니다.
        /// </summary>
        [SerializeField] private Transform oneUnitPosition;
        
        /// <summary>
        /// 2개 유닛 배치 시 사용되는 위치 배열입니다.
        /// </summary>
        [SerializeField] private Transform[] twoUnitPositions = new Transform[2];

        /// <summary>
        /// 3개 유닛 배치 시 사용되는 위치 배열입니다.
        /// </summary>
        [SerializeField] private Transform[] threeUnitPositions = new Transform[3];

        private void Awake()
        {
            AssertHelper.NotNull(typeof(UnitPlacementNode), oneUnitPosition);
            AssertHelper.EqualsValue(typeof(UnitPlacementNode), twoUnitPositions.Length, 2);
            AssertHelper.EqualsValue(typeof(UnitPlacementNode), threeUnitPositions.Length, 3);
        }

        /// <summary>
        /// 주어진 유닛 스폰 요청 데이터에 따라 해당 유닛을 수용할 수 있는지 여부를 반환합니다.
        /// </summary>
        public bool CanAcceptUnit(SUnitSpawnRequestData requestData)
        {
            if (UnitGroup.IsFull())
                return false;

            if (UnitGroup.IsEmpty())
                return true;

            return UnitGroup.UnitType == requestData.UnitType &&
                   UnitGroup.UnitGrade == requestData.UnitGrade;
        }

        /// <summary>
        /// 유닛을 노드에 추가하고, 자동으로 배치 위치를 재조정합니다.
        /// </summary>
        public void AddUnit(UnitRoot unit)
        {
            UnitGroup.AddUnit(unit);
            RearrangeUnitPositions();
        }

        /// <summary>
        /// 노드에 배치된 유닛을 판매하고, 자동으로 배치 위치를 재조정합니다.
        /// </summary>
        public void SubUnit()
        {
            UnitGroup.SubUnit();
            RearrangeUnitPositions();
        }

        /// <summary>
        /// 유닛 합성 이벤트가 일어났을 때 호출되는 메서드입니다.
        /// </summary>
        public void BeforeMergeClearUnit()
        {
            AssertHelper.NotEqualsValue(typeof(UnitPlacementNode), UnitGroup.IsEmpty(), true);
            
            uint unitCount = UnitGroup.UnitCount;
            for (uint i = 0; i < unitCount; ++i)
            {
                UnitGroup.SubUnit();
            }
        }

        /// <summary>
        /// 배치된 유닛들의 위치를 현재 노드 상태에 맞게 재조정합니다.
        /// </summary>
        private void RearrangeUnitPositions()
        {
            switch (UnitGroup.UnitCount)
            {
                case 1:
                    UnitGroup.SetPositions(oneUnitPosition);
                    break;
                case 2:
                    UnitGroup.SetPositions(twoUnitPositions);
                    break;
                case 3:
                    UnitGroup.SetPositions(threeUnitPositions);
                    break;
            }
        }

        /// <summary>
        /// 다른 노드와 유닛 그룹을 교환하고, 서로의 유닛 위치를 재조정한 후 유닛을 이동시킵니다.
        /// </summary>
        public async UniTask SwapWith(UnitPlacementNode targetNode)
        {
            // 유닛 그룹 교환
            (UnitGroup, targetNode.UnitGroup) = (targetNode.UnitGroup, UnitGroup);

            // 서로의 유닛 위치 데이터 재조정
            RearrangeUnitPositions();
            targetNode.RearrangeUnitPositions();

            // 이동할 목표 위치 설정
            Transform[] myTargetPositions = GetUnitPositions();
            Transform[] targetNodePositions = targetNode.GetUnitPositions();

            // 유닛 이동 (모두 이동 완료될 때까지 대기)
            await UniTask.WhenAll(
                UnitGroup.MoveToTargetNode(myTargetPositions),
                targetNode.UnitGroup.MoveToTargetNode(targetNodePositions)
            );
        }
        
        /// <summary>
        /// 현재 UnitGroup의 유닛 수에 맞는 위치 배열을 반환합니다.
        /// </summary>
        private Transform[] GetUnitPositions()
        {
            return UnitGroup.UnitCount switch
            {
                1 => new[] { oneUnitPosition },
                2 => twoUnitPositions,
                3 => threeUnitPositions,
                _ => Array.Empty<Transform>() // 예외 방지
            };
        }
    }
}
