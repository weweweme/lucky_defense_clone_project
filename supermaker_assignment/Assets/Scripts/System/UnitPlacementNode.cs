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
        /// <summary>
        /// 최대 배치 가능한 유닛 수입니다.
        /// </summary>
        private const int MAX_UNIT_COUNT = 3;
        
        /// <summary>
        /// 현재 노드가 유닛으로 점유되어 있는지 여부입니다.
        /// </summary>
        private bool _isOccupied;

        /// <summary>
        /// 현재 배치된 유닛의 등급입니다.
        /// </summary>
        private EUnitGrade _unitGrade;

        /// <summary>
        /// 현재 배치된 유닛의 타입입니다.
        /// </summary>
        private EUnitType _unitType;

        /// <summary>
        /// 현재 노드에 배치된 유닛 수입니다.
        /// </summary>
        private uint _unitCount;

        /// <summary>
        /// 해당 노드에 배치된 유닛들의 참조 배열입니다.
        /// </summary>
        [SerializeField] private UnitRoot[] _placedUnits = new UnitRoot[MAX_UNIT_COUNT];

        /// <summary>
        /// 2개 유닛 배치 시 사용되는 위치 배열입니다.
        /// </summary>
        [SerializeField] private Transform[] twoUnitPositions = new Transform[2];

        /// <summary>
        /// 3개 유닛 배치 시 사용되는 위치 배열입니다.
        /// </summary>
        [SerializeField] private Transform[] threeUnitPositions = new Transform[3];

        /// <summary>
        /// 주어진 유닛 스폰 요청 데이터에 따라 해당 유닛을 수용할 수 있는지 여부를 반환합니다.
        /// </summary>
        public bool CanAcceptUnit(SUnitSpawnRequestData requestData)
        {
            // 이미 가득 찼거나 신화 등급인 경우 추가 불가
            if (_unitCount == 3 || _unitGrade == EUnitGrade.Mythic)
                return false;

            // 비어있으면 바로 배치 가능
            if (!_isOccupied)
                return true;

            // 동일 타입 & 동일 등급이면 합체 가능
            return _unitType == requestData.UnitType && _unitGrade == requestData.UnitGrade;
        }
        
        /// <summary>
        /// 유닛을 노드에 추가하고, 자동으로 배치 위치를 재조정합니다.
        /// </summary>
        public void AddUnit(UnitRoot unit)
        {
            Debug.Assert(_unitCount != 3);

            _placedUnits[_unitCount++] = unit;
            RearrangeUnitPositions();
        }

        /// <summary>
        /// 배치된 유닛들의 위치를 현재 노드 상태에 맞게 재조정합니다.
        /// </summary>
        private void RearrangeUnitPositions()
        {
            if (_unitCount == 1)
            {
                _placedUnits[0].transform.position = transform.position;
                return;
            }

            Transform[] targetPositions = _unitCount == 2 ? twoUnitPositions : threeUnitPositions;

            for (int i = 0; i < _unitCount; ++i)
            {
                _placedUnits[i].transform.position = targetPositions[i].position;
            }
        }
    }
}
