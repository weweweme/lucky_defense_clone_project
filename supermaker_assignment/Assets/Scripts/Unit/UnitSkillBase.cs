using UnityEngine;
using Util;

namespace Unit
{
    /// <summary>
    /// 유닛 스킬 컴포넌트의 베이스 클래스입니다.
    /// 모든 유닛 스킬은 이 클래스를 상속받아 구현되며, 
    /// 특정 타겟을 설정하고 스킬을 사용하는 기능을 제공합니다.
    /// </summary>
    public abstract class UnitSkillBase : MonoBehaviourBase
    {
        /// <summary>
        /// 현재 스킬이 지정된 타겟의 Transform입니다.
        /// 스킬의 대상이 존재하지 않을 수도 있습니다.
        /// </summary>
        protected Transform targetTr;

        /// <summary>
        /// 스킬의 타겟을 설정하는 메서드입니다.
        /// 유닛의 공격 대상 또는 특정 조건에 따라 설정됩니다.
        /// </summary>
        public abstract void SetTarget();

        /// <summary>
        /// 스킬을 발동하는 메서드입니다.
        /// 구체적인 스킬 효과는 하위 클래스에서 구현됩니다.
        /// </summary>
        public abstract void UseSkill();
    }
}
