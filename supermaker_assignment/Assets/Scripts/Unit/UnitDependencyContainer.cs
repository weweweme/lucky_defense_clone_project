using System;
using Model;

namespace Unit
{
    /// <summary>
    /// Unit에서 사용될 외부 모듈의 참조를 관리하는 컨테이너 클래스입니다.
    /// </summary>
    public sealed class UnitDependencyContainer : DependencyContainerBase
    {
        internal readonly UnitBasePool unitBasePool;
        internal readonly MDL_Unit mdlUnit;
        internal readonly MDL_Currency mdlCurrency;
        internal readonly MDL_UnitResources mdlUnitResources;

        public UnitDependencyContainer(RootManager rootManager)
        {
            unitBasePool = rootManager.PoolManager.UnitBasePool;
            mdlUnit = rootManager.DataManager.Unit;
            mdlCurrency = rootManager.DataManager.Currency;
            mdlUnitResources = rootManager.DataManager.UnitResources;
        }
    }
}
