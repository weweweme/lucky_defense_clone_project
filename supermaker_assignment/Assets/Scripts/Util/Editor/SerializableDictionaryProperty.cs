using UnityEditor;

namespace Util.Editor
{
    [CustomPropertyDrawer(typeof(MythicUnitFullSpriteDic))]
    [CustomPropertyDrawer(typeof(UnitGradeToGambleMetaDataDic))]
    
    public class SerializableDictionaryDrawer : SerializableDictionaryPropertyDrawer {}
}
