using System;
using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// 적의 메타 데이터입니다.
    /// </summary>
    [Serializable]
    public class EnemyMetaData
    {
        public EUnitType Type;
        public Sprite Sprite;
    }
}
