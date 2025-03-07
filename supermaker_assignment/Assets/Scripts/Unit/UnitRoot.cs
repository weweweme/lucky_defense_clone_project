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
        private UnitBTController _btController;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(UnitRoot), attackController);
        }
        
        public void CreatePooledItemInit()
        {
            _btController = new UnitBTController(this);
            attackController.Init();
        }

        public override void OnTakeFromPoolInit(EPlayerSide side)
        {
            _grade = EUnitGrade.Common;
            _type = EUnitType.Melee;
            
            _btController.StartBtTick();
        }
        
        public void ReleaseObject()
        {
        }
    }
}
