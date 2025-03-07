using System;
using Enemy;
using Util;

namespace Unit
{
    /// <summary>
    /// 유닛 오브젝트 베이스를 관리하는 풀 클래스입니다.
    /// </summary>
    public class UnitBasePool : ObjectPoolBase<UnitRoot>
    {
        protected override UnitRoot CreatePooledItem()
        {
            // TODO: UnitDependencyContainer 클래스 구현 뒤 코드 정상화
            EnemyDependencyContainer tmp = new EnemyDependencyContainer(RootManager.Ins);
            
            var enemy = base.CreatePooledItem();
            enemy.CreatePooledItemInit(tmp);

            return enemy;
        }
    }
}
