using System;
using Unit;
using UnityEngine;
using Util;

namespace UI
{
    /// <summary>
    /// 유닛 스킬 풀들을 관리하고, 유닛의 등급과 타입에 맞는 스킬 객체를 반환하는 클래스입니다.
    /// </summary>
    public sealed class UnitSkillPoolManager : MonoBehaviourBase
    {
        [SerializeField] private UnitSkillPool _rareRangeSkillPool;
        [SerializeField] private UnitSkillPool _rareMeleeSkillPool;
        [SerializeField] private UnitSkillPool _heroRangeSkillPool;
        [SerializeField] private UnitSkillPool _heroMeleeSkillPool;
        [SerializeField] private UnitSkillPool _mythicMeleeSkillPool;
        [SerializeField] private UnitSkillPool _mythicRangeSkillPool;
        
        private void Awake()
        {
            AssertHelper.NotNull(typeof(UnitSkillPoolManager), _rareRangeSkillPool);
            // AssertHelper.NotNull(typeof(UnitSkillPoolManager), _rareMeleeSkillPool);
            // AssertHelper.NotNull(typeof(UnitSkillPoolManager), _heroRangeSkillPool);
            // AssertHelper.NotNull(typeof(UnitSkillPoolManager), _heroMeleeSkillPool);
        }

        /// <summary>
        /// 유닛의 등급과 타입에 맞는 스킬 객체를 반환합니다.
        /// </summary>
        /// <param name="grade">유닛의 등급</param>
        /// <param name="type">유닛의 타입</param>
        /// <returns>적절한 UnitSkillBase 객체</returns>
        public UnitSkillBase GetSkill(EUnitGrade grade, EUnitType type)
        {
            return (grade, type) switch
            {
                (EUnitGrade.Rare, EUnitType.Ranged) => _rareRangeSkillPool.GetObject(),
                // (EUnitGrade.Rare, EUnitType.Melee) => _rareMeleeSkillPool.GetObject(),
                // (EUnitGrade.Hero, EUnitType.Ranged) => _heroRangeSkillPool.GetObject(),
                // (EUnitGrade.Hero, EUnitType.Melee) => _heroMeleeSkillPool.GetObject(),
                // (EUnitGrade.Mythic, EUnitType.Melee) => _mythicMeleeSkillPool.GetObject(),
                // (EUnitGrade.Mythic, EUnitType.Ranged) => _mythicRangeSkillPool.GetObject(),
                _ => null // 신화 등급 스킬은 별도 관리
            };
        }
    }
}
