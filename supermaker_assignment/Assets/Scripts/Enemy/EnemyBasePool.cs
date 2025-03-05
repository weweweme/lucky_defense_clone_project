using Util;

namespace Enemy
{
    /// <summary>
    /// 적 오브젝트 베이스를 관리하는 풀링 클래스입니다.
    /// </summary>
    public sealed class EnemyBasePool : ObjectPoolBase<EnemyRoot>
    {
        protected override EnemyRoot CreatePooledItem()
        {
            var enemy = base.CreatePooledItem();
            enemy.Init();

            return enemy;
        }
    }
}
