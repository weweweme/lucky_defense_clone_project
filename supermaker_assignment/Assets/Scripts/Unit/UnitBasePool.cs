using System;
using Util;

namespace Unit
{
    /// <summary>
    /// 유닛 오브젝트 베이스를 관리하는 풀 클래스입니다.
    /// </summary>
    public class UnitBasePool : ObjectPoolBase<UnitRoot>
    {
        private UnitDependencyContainer _dependencyContainer;

        public void Init(RootManager rootManager)
        {
            _dependencyContainer = new UnitDependencyContainer(rootManager);
        }
        
        protected override UnitRoot CreatePooledItem()
        {
            var enemy = base.CreatePooledItem();
            enemy.CreatePooledItemInit(_dependencyContainer);

            return enemy;
        }
    }
}
