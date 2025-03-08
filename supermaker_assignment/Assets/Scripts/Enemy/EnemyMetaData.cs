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
        public EEnemyType Type;
        public Sprite Sprite;
        public float ScaleSize;
    }
}
