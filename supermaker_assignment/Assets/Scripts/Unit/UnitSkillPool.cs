using Util;

namespace Unit
{
    /// <summary>
    /// 유닛의 스킬을 관리하는 풀 클래스입니다
    /// </summary>
    public sealed class UnitSkillPool : ObjectPoolBase<UnitSkillBase>
    {
        protected override UnitSkillBase CreatePooledItem()
        {
            var enemy = base.CreatePooledItem();
            enemy.CreatePooledItemInit(this);
            
            return base.CreatePooledItem();
        }
    }
}
