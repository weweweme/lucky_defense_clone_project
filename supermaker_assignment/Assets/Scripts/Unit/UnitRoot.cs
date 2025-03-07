using System;
using UnityEngine;
using Util;

namespace Unit
{
    /// <summary>
    /// Unit의 최상위 루트 클래스입니다.
    /// 각 모듈의 참조를 관리하고, 초기화 과정을 수행합니다.
    /// </summary>
    public class UnitRoot : PooledEntityRootBase
    {
        private EUnitGrade _grade = EUnitGrade.None;
        public EUnitGrade Grade => _grade;
        
        private EUnitType _type = EUnitType.None;
        public EUnitType Type => _type;
        
        [SerializeField] internal UnitAttackController attackController;
        [SerializeField] internal UnitSpriteController spriteController;
        internal UnitDependencyContainer dependencyContainer;
        private UnitBTController _btController;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(UnitRoot), attackController);
            AssertHelper.NotNull(typeof(UnitRoot), spriteController);
        }
        
        public override void CreatePooledItemInit(DependencyContainerBase containerBase)
        {
            UnitDependencyContainer container = containerBase as UnitDependencyContainer;
            AssertHelper.NotNull(typeof(UnitRoot), container);
            
            dependencyContainer = container;
            _btController = new UnitBTController(this);
            spriteController.Init(this);
            attackController.Init();
        }
        
        public void SetupUnitClassification(EUnitGrade grade, EUnitType type)
        {
            AssertHelper.NotEqualsEnum(typeof(EnemySpawnHandler),grade, EUnitGrade.None);
            AssertHelper.NotEqualsEnum(typeof(EnemySpawnHandler),type, EUnitType.None);
            
            _grade = grade;
            _type = type;
        }

        public override void OnTakeFromPoolInit(EPlayerSide side)
        {
            AssertHelper.NotEqualsEnum(typeof(EnemySpawnHandler),side, EPlayerSide.None);
            AssertHelper.NotEqualsEnum(typeof(EnemySpawnHandler),_grade, EUnitGrade.None);
            AssertHelper.NotEqualsEnum(typeof(EnemySpawnHandler),_type, EUnitType.None);
            
            spriteController.ChangeVisible();
            _btController.StartBtTick();
            attackController.OnTakeFromPoolInit(this);
        }
        
        public void ReleaseObject()
        {
            _btController.Dispose();
            dependencyContainer.unitBasePool.ReleaseObject(this);
        }
    }
}
