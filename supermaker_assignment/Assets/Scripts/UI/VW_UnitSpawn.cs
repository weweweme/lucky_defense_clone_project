using InGame.System;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    /// <summary>
    /// Unit Spawn UI의 View 클래스입니다.
    /// </summary>
    public sealed class VW_UnitSpawn : View
    {
        [SerializeField] internal Button btnSpawn;
        
        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_UnitSpawn), btnSpawn);
        }
    }
}
