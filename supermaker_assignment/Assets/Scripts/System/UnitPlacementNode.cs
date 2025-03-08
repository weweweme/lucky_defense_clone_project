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
        public void SellUnit()
        {
            UnitGroup.SubUnit();
            RearrangeUnitPositions();
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
        /// 다른 노드와 유닛 그룹을 교환하고, 서로의 유닛 위치를 재조정합니다.
        /// </summary>
        public void SwapWith(UnitPlacementNode targetNode)
        {
            (UnitGroup, targetNode.UnitGroup) = (targetNode.UnitGroup, UnitGroup);
            RearrangeUnitPositions();
            targetNode.RearrangeUnitPositions();
        }
    }
}
