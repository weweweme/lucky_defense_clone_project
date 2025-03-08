using UnityEditor;

namespace Util.Editor
{
    [CustomPropertyDrawer(typeof(MythicUnitFullSpriteDic))]
    [CustomPropertyDrawer(typeof(UnitGradeToGambleMetaDataDic))]
    [CustomPropertyDrawer(typeof(UnitGradeToMaterialDic))]
    
    public class SerializableDictionaryDrawer : SerializableDictionaryPropertyDrawer {}
}
