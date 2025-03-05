using InGame.System;
using TMPro;
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
        [SerializeField] internal TextMeshProUGUI currentCurrency; 
        
        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_UnitSpawn), btnSpawn);
            AssertHelper.NotNull(typeof(VW_UnitSpawn), currentCurrency);
        }
    }
}
