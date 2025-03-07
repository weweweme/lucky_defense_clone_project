using System;
using UnityEngine;

namespace Unit
{
    /// <summary>
    /// 유닛의 메타 데이터입니다.
    /// </summary>
    [Serializable]
    public class UnitMetaData
    {
        public EUnitGrade Grade;
        public EUnitType Type;
        public uint SpawnCost;
        public uint SellPrice;
        public Sprite Sprite;
    }
}
