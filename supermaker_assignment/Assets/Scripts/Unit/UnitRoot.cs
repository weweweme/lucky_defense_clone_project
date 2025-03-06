using System;

namespace Unit
{
    /// <summary>
    /// Unit의 최상위 루트 클래스입니다.
    /// 각 모듈의 참조를 관리하고, 초기화 과정을 수행합니다.
    /// </summary>
    public class UnitRoot : PooledEntityRootBase
    {
        // TODO: UnitGrade와 Type이 OnTakeFromPoolInit에서 초기화되도록 설정합니다.
        private EUnitGrade _grade = EUnitGrade.Common;
        public EUnitGrade Grade => _grade;
        
        private EUnitType _type = EUnitType.Melee;
        public EUnitType Type => _type;
        
        public override void OnTakeFromPoolInit(EPlayerSide side)
        {
        }
    }
}
