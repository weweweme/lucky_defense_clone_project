using UI;
using Util;

namespace Unit
{
    /// <summary>
    /// 유닛의 스킬을 관리하고 사용하는 컴포넌트입니다.
    /// </summary>
    public sealed class UnitSkillController : MonoBehaviourBase
    {
        private UnitSkillBase _skill;
        private UnitSkillPoolManager _unitSkillPoolManager;
        private UnitAttackController _unitAttackController;

        public void CreatePooledItemInit(UnitRoot root)
        {
            _unitSkillPoolManager = root.dependencyContainer.unitSkillPoolManager;
            _unitAttackController = root.attackController;
        }

        public void SetCurrentSkillGrade(UnitRoot root)
        {
            _skill = _unitSkillPoolManager.GetSkill(root.grade, root.type);
            _skill.SetStartPoint(transform);
        }

        /// <summary>
        /// 스킬을 사용합니다.
        /// </summary>
        /// <param name="skill">사용할 스킬</param>
        public void UseSkill()
        {
            _skill.SetStartPoint(_unitAttackController.CurrentTarget.Transform);
            _skill.UseSkill();
        }
    }
}