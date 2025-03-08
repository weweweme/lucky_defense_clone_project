using System;
using UI;
using UnityEngine;

namespace Util
{
    [Serializable]
    public class MythicUnitFullSpriteDic : SerializableDictionary<EUnitType, Sprite> { }
    
    [Serializable]
    public class UnitGradeToGambleMetaDataDic : SerializableDictionary<EUnitGrade, GambleChoiceItem> { }
    
    [Serializable]
    public class UnitGradeToMaterialDic : SerializableDictionary<EUnitGrade, Material> { }
}
