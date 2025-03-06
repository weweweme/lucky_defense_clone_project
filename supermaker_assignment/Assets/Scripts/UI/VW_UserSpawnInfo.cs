using InGame.System;
using TMPro;
using UnityEngine;
using Util;

namespace UI
{
    /// <summary>
    /// User Spawn Info와 관련된 UI의 동작을 수행하는 View 클래스입니다.
    /// </summary>
    public sealed class VW_UserSpawnInfo : View
    {
        [SerializeField] private TextMeshProUGUI maxSpawnCount;
        [SerializeField] private TextMeshProUGUI currentSpawnCount;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_UserSpawnInfo), maxSpawnCount);
            AssertHelper.NotNull(typeof(VW_UserSpawnInfo), currentSpawnCount);
        }
        
        public void UpdateMaxSpawnCount(uint maxCount) => maxSpawnCount.SetText(maxCount.ToString());
        public void UpdateCurrentSpawnCount(uint currentCount) => currentSpawnCount.SetText(currentCount.ToString());
    }
}
