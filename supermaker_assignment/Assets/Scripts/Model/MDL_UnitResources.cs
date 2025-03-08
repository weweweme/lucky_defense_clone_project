using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unit;
using UnityEngine;
using Util;

namespace Model
{
    /// <summary>
    /// 프로젝트 내부에서 사용되는 유닛 관련 리소스들을 관리하는 클래스입니다.
    /// </summary>
    public sealed class MDL_UnitResources : MonoBehaviourBase
    {
        private const uint CURRENT_UNIT_COUNT = 8;
        [SerializeField] private List<UnitMetaData> _unitResourceList;
        private readonly Dictionary<EUnitGrade, Dictionary<EUnitType, UnitMetaData>> _unitResourceDic = new();
        
        [SerializeField] private MythicUnitFullSpriteDic _mythicUnitFullSpriteDic;
        [SerializeField] private UnitGradeToMaterialDic _unitGradeToMaterialDic;

        private void Awake()
        {
            AssertHelper.EqualsValue(typeof(MDL_UnitResources), (uint)_unitResourceList.Count, CURRENT_UNIT_COUNT);
            
            InitResourceDic();
        }

        private void InitResourceDic()
        {
            _unitResourceDic.Clear();

            foreach (var data in _unitResourceList)
            {
                if (!_unitResourceDic.TryGetValue(data.Grade, out var typeDic))
                {
                    typeDic = new Dictionary<EUnitType, UnitMetaData>();
                    _unitResourceDic[data.Grade] = typeDic;
                }
                
                typeDic[data.Type] = data;
            }
        }

        [NotNull]
        public UnitMetaData GetResource(EUnitGrade grade, EUnitType type)
        {
            AssertHelper.NotEqualsEnum(typeof(MDL_UnitResources), grade, EUnitGrade.None);
            AssertHelper.NotEqualsEnum(typeof(MDL_UnitResources), type, EUnitType.None);

            if (_unitResourceDic.TryGetValue(grade, out var typeDic) && typeDic.TryGetValue(type, out var data))
            {
                return data;
            }

            throw new KeyNotFoundException($"리소스 데이터 없음: {grade}-{type}");
        }

        [NotNull]
        public Sprite GetMythicUnitFullSprite(EUnitType type)
        {
            AssertHelper.NotEqualsEnum(typeof(MDL_UnitResources), type, EUnitType.None);

            if (_mythicUnitFullSpriteDic.TryGetValue(type, out var sprite))
            {
                return sprite;
            }

            throw new KeyNotFoundException($"신화 유닛 전체 이미지 리소스 없음: {type}");
        }
    }
}
