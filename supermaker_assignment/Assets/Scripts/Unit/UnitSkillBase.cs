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
        /// 스킬 발동을 시작하는 위치입니다.
        /// 유닛의 현재 위치 또는 스킬이 생성되는 위치로 사용됩니다.
        /// </summary>
        protected Transform startTr;
        
        /// <summary>
        /// 현재 스킬이 지정된 타겟의 Transform입니다.
        /// 스킬의 대상이 존재하지 않을 수도 있습니다.
        /// </summary>
        protected Transform targetTr;
        
        /// <summary>
        /// 스킬 오브젝트를 관리하는 풀의 참조입니다.
        /// 스킬을 사용한 후 반환할 때 활용됩니다.
        /// </summary>
        protected UnitSkillPool skillPool;
        
        /// <summary>
        /// 스킬이 생성된 후 호출되며, 해당 스킬이 속한 풀을 설정합니다.
        /// </summary>
        /// <param name="pool">스킬 오브젝트 풀</param>
        public void CreatePooledItemInit(UnitSkillPool pool) => skillPool = pool;

        /// <summary>
        /// 스킬의 시작 위치를 설정합니다.
        /// 일반적으로 유닛의 현재 위치를 기반으로 설정됩니다.
        /// </summary>
        /// <param name="start">시작 지점 Transform</param>
        public void SetStartPoint(Transform start) => startTr = start;
        
        /// <summary>
        /// 스킬의 타겟을 설정합니다.
        /// 공격 대상 또는 특정 효과를 부여할 대상을 지정하는 데 사용됩니다.
        /// </summary>
        /// <param name="target">타겟 Transform</param>
        public void SetTarget(Transform target) => targetTr = target;
        
        /// <summary>
        /// 스킬을 사용하고 정보를 비웁니다.
        /// </summary>
        public void ClearRef()
        {
            targetTr = null;
            startTr = null;
        }

        /// <summary>
        /// 스킬을 발동하는 메서드입니다.
        /// 구체적인 스킬 효과는 하위 클래스에서 구현됩니다.
        /// </summary>
        public abstract void UseSkill();
    }
}
