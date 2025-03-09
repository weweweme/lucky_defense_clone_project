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
        internal EUnitGrade grade = EUnitGrade.None;
        internal EUnitType type = EUnitType.None;
        
        [SerializeField] internal UnitAttackController attackController;
        [SerializeField] internal UnitSpriteController spriteController;
        internal UnitDependencyContainer dependencyContainer;
        
        private UnitBTController _btController;
        [SerializeField] private UnitMoveController _moveController;
        public UnitMoveController MoveController => _moveController;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(UnitRoot), attackController);
            AssertHelper.NotNull(typeof(UnitRoot), spriteController);
            AssertHelper.NotNull(typeof(UnitRoot), _moveController);
        }
        
        public override void CreatePooledItemInit(DependencyContainerBase containerBase)
        {
            UnitDependencyContainer container = containerBase as UnitDependencyContainer;
            AssertHelper.NotNull(typeof(UnitRoot), container);
            
            dependencyContainer = container;
            _btController = new UnitBTController(this);
            spriteController.CreatePooledItemInit(this);
            attackController.CreatePooledItemInit(this);
            _moveController.CreatePooledItemInit(this);
        }
        
        public void SetupUnitClassification(EUnitGrade unitGrade, EUnitType unitType)
        {
            AssertHelper.NotEqualsEnum(typeof(EnemySpawnHandler),unitGrade, EUnitGrade.None);
            AssertHelper.NotEqualsEnum(typeof(EnemySpawnHandler),unitType, EUnitType.None);
            
            grade = unitGrade;
            type = unitType;
        }

        public override void OnTakeFromPoolInit(EPlayerSide side)
        {
            AssertHelper.NotEqualsEnum(typeof(EnemySpawnHandler),side, EPlayerSide.None);
            AssertHelper.NotEqualsEnum(typeof(EnemySpawnHandler),grade, EUnitGrade.None);
            AssertHelper.NotEqualsEnum(typeof(EnemySpawnHandler),type, EUnitType.None);
            
            spriteController.ChangeVisible();
            _btController.StartBtTick();
            attackController.ChangeAttackData(this);
            _moveController.ChangeEffectScale();
        }
        
        public void ReleaseObject()
        {
            _btController.Dispose();
            dependencyContainer.unitBasePool.ReleaseObject(this);
        }
    }
}
