using CleverCrow.Fluid.BTs.Tasks;
using UI;
using Util;
using Cysharp.Threading.Tasks;

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

        private float _skillCooldownDuration; // 스킬 쿨타임
        private float _skillCooldown; // 현재 남은 쿨타임

        public void CreatePooledItemInit(UnitRoot root)
        {
            _unitSkillPoolManager = root.dependencyContainer.unitSkillPoolManager;
            _unitAttackController = root.attackController;
        }

        public void SetCurrentSkillGrade(UnitRoot root)
        {
            _skill = _unitSkillPoolManager.GetSkill(root.grade, root.type);
            
            if (_skill == null) return;

            var metaData = root.dependencyContainer.mdlUnitResources.GetResource(root.grade, root.type);
            float skillCooldownDuration = metaData.SkillCoolTime;
            _skillCooldownDuration = skillCooldownDuration;
            SetCooldown();
            _skill.SetOwned(root);
        }

        /// <summary>
        /// 스킬을 사용합니다.
        /// </summary>
        public TaskStatus UseSkill()
        {
            _skill.SetStartPoint(transform);
            _skill.SetTarget(_unitAttackController.CurrentTarget.Transform);
            _skill.UseSkill();
            
            SetCooldown(); // 스킬 사용 후 쿨타임 시작
            return TaskStatus.Success;
        }

        /// <summary>
        /// 스킬 쿨타임을 설정합니다.
        /// </summary>
        private void SetCooldown()
        {
            _skillCooldown = _skillCooldownDuration;
            StartCooldownTimer().Forget();
        }
        
        public void ClearOwned()
        {
            if (_skill == null) return;
            _skill.ClearOwned();
        }

        /// <summary>
        /// 쿨타임을 비동기적으로 감소시킵니다.
        /// </summary>
        private async UniTaskVoid StartCooldownTimer()
        {
            while (_skillCooldown > 0)
            {
                await UniTask.Delay(100, cancellationToken: this.GetCancellationTokenOnDestroy());
                _skillCooldown -= 0.1f;
            }
        }

        /// <summary>
        /// 스킬이 사용 가능한 상태인지 확인합니다.
        /// </summary>
        public bool IsSkillReady()
        {
            if (_skill == null) return false;
            return _skillCooldown <= 0;
        }
    }
}
