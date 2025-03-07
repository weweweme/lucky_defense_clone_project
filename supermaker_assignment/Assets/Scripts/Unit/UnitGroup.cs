using System;
using UnityEngine;
using Util;

namespace Unit
{
    /// <summary>
    /// 유닛 그룹을 관리하는 클래스입니다.
    /// 하나의 배치 노드에 소속된 유닛들의 집합과 정보를 관리합니다.
    /// </summary>
    public class UnitGroup
    {
        /// <summary>
        /// 최대 배치 가능한 유닛 수입니다.
        /// </summary>
        private const int MAX_UNIT_COUNT = 3;
        
        /// <summary>
        /// 해당 노드에 배치된 유닛들의 참조 배열입니다.
        /// </summary>
        private readonly UnitRoot[] _placedUnits = new UnitRoot[MAX_UNIT_COUNT];

        /// <summary>
        /// 현재 그룹에 속한 유닛들의 등급입니다.
        /// </summary>
        public EUnitGrade UnitGrade { get; private set; }

        /// <summary>
        /// 현재 그룹에 속한 유닛들의 타입입니다.
        /// </summary>
        public EUnitType UnitType { get; private set; }

        /// <summary>
        /// 현재 그룹에 속한 유닛 수입니다.
        /// </summary>
        public uint UnitCount { get; private set; }

        /// <summary>
        /// 유닛을 그룹에 추가합니다.
        /// </summary>
        public void AddUnit(UnitRoot unit)
        {
            AssertHelper.NotEqualsValue<uint>(typeof(UnitGroup), UnitCount, MAX_UNIT_COUNT);

            _placedUnits[UnitCount++] = unit;
            UnitGrade = unit.Grade;
            UnitType = unit.Type;
        }

        /// <summary>
        /// 마지막으로 추가된 유닛을 판매합니다.
        /// </summary>
        public void SellUnit()
        {
            UnitRoot unit = _placedUnits[UnitCount--];
            AssertHelper.NotNull(typeof(UnitGroup), unit);
            unit.ReleaseObject();

            if (!IsEmpty()) return;
            Clear();
        }

        /// <summary>
        /// 현재 배치된 유닛들의 위치를 지정된 위치로 설정합니다.
        /// </summary>
        public void SetPositions(params Transform[] targetPositions)
        {
            for (int i = 0; i < UnitCount; ++i)
            {
                _placedUnits[i].transform.position = targetPositions[i].position;
            }
        }

        /// <summary>
        /// 그룹의 모든 유닛 정보를 제거하고 초기화합니다.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < MAX_UNIT_COUNT; ++i)
            {
                _placedUnits[i] = null;
            }

            UnitGrade = EUnitGrade.None;
            UnitType = EUnitType.None;
            UnitCount = 0;
        }

        /// <summary>
        /// 현재 그룹이 가득 찼는지 여부를 반환합니다.
        /// </summary>
        public bool IsFull() => UnitCount == MAX_UNIT_COUNT || UnitGrade == EUnitGrade.Mythic;
        
        /// <summary>
        /// 현재 그룹이 비어있는지 여부를 반환합니다.
        /// </summary>
        /// <returns>true일 경우 비어있음, false일 경우 점유 중 </returns>
        public bool IsEmpty() => UnitCount == 0;

        /// <summary>
        /// 현재 그룹의 공격 사정거리를 반환합니다.
        /// </summary>
        /// <returns>현재 그룹의 공격 사정거리.</returns>
        public float GetAttackRange()
        {
            // TODO: 추후 유닛의 타입에 따라 사정거리 분기 기능 추가.
            return 5.0f;
        }
    }
}
