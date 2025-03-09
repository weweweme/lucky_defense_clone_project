using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
        /// 해당 노드에 배치된 유닛들의 리스트입니다.
        /// </summary>
        private readonly List<UnitRoot> _placedUnits = new List<UnitRoot>(MAX_UNIT_COUNT);

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
        public int UnitCount => _placedUnits.Count; // 리스트 크기 기반으로 갱신
        
        /// <summary>
        /// 유닛 이동 작업을 저장하는 캐싱된 배열 (최대 3개까지 저장)
        /// </summary>
        private readonly UniTask[] _moveTasks = new UniTask[MAX_UNIT_COUNT];

        /// <summary>
        /// 유닛을 그룹에 추가합니다.
        /// </summary>
        public void AddUnit(UnitRoot unit)
        {
            AssertHelper.NotEqualsValue(typeof(UnitGroup), UnitCount, MAX_UNIT_COUNT);
            
            _placedUnits.Add(unit);
            UpdateUnitInfo();
        }

        /// <summary>
        /// ✅ 마지막으로 추가된 유닛을 제거합니다.
        /// </summary>
        public void SubUnit()
        {
            if (IsEmpty()) return;

            int lastIndex = _placedUnits.Count - 1;
            UnitRoot unit = _placedUnits[lastIndex];

            AssertHelper.NotNull(typeof(UnitGroup), unit);
            unit.ReleaseObject();

            _placedUnits.RemoveAt(lastIndex); // 마지막 유닛 제거

            if (!IsEmpty()) 
            {
                UpdateUnitInfo(); // 남아 있는 유닛 기준으로 UnitType, UnitGrade 갱신
                return;
            }

            Clear();
        }

        /// <summary>
        /// 현재 배치된 유닛의 정보를 다시 설정 (UnitType, UnitGrade 갱신)
        /// </summary>
        public void UpdateUnitInfo()
        {
            if (IsEmpty())
            {
                Clear();
                return;
            }

            UnitGrade = _placedUnits[0].grade;
            UnitType = _placedUnits[0].type;
        }

        /// <summary>
        /// 그룹의 모든 유닛 정보를 제거하고 초기화합니다.
        /// </summary>
        private void Clear()
        {
            _placedUnits.Clear(); // 리스트 비우기
            UnitGrade = EUnitGrade.None;
            UnitType = EUnitType.None;
        }

        /// <summary>
        /// 현재 그룹이 가득 찼는지 여부를 반환합니다.
        /// </summary>
        public bool IsFull() => UnitCount >= MAX_UNIT_COUNT || UnitGrade == EUnitGrade.Mythic;
        
        /// <summary>
        /// 현재 그룹이 비어있는지 여부를 반환합니다.
        /// </summary>
        public bool IsEmpty() => _placedUnits.Count == 0;

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
        /// 현재 배치된 유닛들을 지정된 위치로 이동시킨 후, 모두 도착할 때까지 기다립니다.
        /// </summary>
        public async UniTask MoveToTargetNode(params Transform[] targetPositions)
        {
            for (int i = 0; i < MAX_UNIT_COUNT; ++i)
            {
                _moveTasks[i] = default;
            }
            
            for (int i = 0; i < UnitCount; ++i)
            {
                _moveTasks[i] = _placedUnits[i].MoveController.MoveToTarget(targetPositions[i]);
            }

            await UniTask.WhenAll(_moveTasks);
        }
        
        public float GetAttackRange()
        {
            UnitMetaData data = RootManager.Ins.DataManager.UnitResources.GetResource(UnitGrade, UnitType);
            AssertHelper.NotNull(typeof(UnitGroup), data);

            return data.AttackRange;
        }
    }
}
