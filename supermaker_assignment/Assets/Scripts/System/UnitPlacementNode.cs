using UnityEngine;

namespace System
{
    /// <summary>
    /// 유닛이 배치되는 단일 노드를 나타내는 클래스입니다.
    /// 유닛의 등급, 타입, 개수 및 다인 배치 시 위치 정보를 관리합니다.
    /// </summary>
    public sealed class UnitPlacementNode : MonoBehaviour
    {
        /// <summary>
        /// 현재 노드에 유닛이 배치되어 있는지 여부입니다.
        /// </summary>
        public bool IsOccupied { get; private set; }

        /// <summary>
        /// 현재 배치된 유닛의 등급입니다.
        /// </summary>
        public EUnitGrade UnitGrade { get; private set; }

        /// <summary>
        /// 현재 배치된 유닛의 타입입니다.
        /// </summary>
        public EUnitType UnitType { get; private set; }

        /// <summary>
        /// 현재 배치된 유닛의 수입니다.
        /// </summary>
        public uint UnitCount { get; private set; }

        /// <summary>
        /// 두 명이 배치될 경우, 각 유닛이 위치할 Transform 배열입니다.
        /// </summary>
        [SerializeField] private Transform[] twoUnitPositions = new Transform[2];

        /// <summary>
        /// 세 명이 배치될 경우, 각 유닛이 위치할 Transform 배열입니다.
        /// </summary>
        [SerializeField] private Transform[] threeUnitPositions = new Transform[3];

        /// <summary>
        /// 노드 상태를 초기화합니다.
        /// </summary>
        public void ResetNode()
        {
            IsOccupied = false;
            UnitGrade = EUnitGrade.None;
            UnitType = EUnitType.None;
            UnitCount = 0;
        }

        /// <summary>
        /// 유닛 배치 정보를 설정합니다.
        /// </summary>
        public void SetUnitData(EUnitGrade grade, EUnitType type, uint count)
        {
            IsOccupied = true;
            UnitGrade = grade;
            UnitType = type;
            UnitCount = count;
        }

        /// <summary>
        /// 현재 유닛 수에 맞는 위치 배열을 반환합니다.
        /// </summary>
        public Transform[] GetUnitPositions()
        {
            if (UnitCount == 2)
            {
                return twoUnitPositions;
            }
            if (UnitCount == 3)
            {
                return threeUnitPositions;
            }
            return new Transform[] { transform }; // 기본적으로 한 명일 때 자기 자신 위치 반환
        }
    }
}
