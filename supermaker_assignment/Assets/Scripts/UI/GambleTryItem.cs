using TMPro;
using UnityEngine;
using Util;

namespace UI
{
    /// <summary>
    /// 갬블 시도 UI 클래스입니다.
    /// </summary>
    public sealed class GambleTryItem : MonoBehaviourBase
    {
        [SerializeField] internal TextMeshProUGUI chancePercent;
        [SerializeField] internal TextMeshProUGUI tryNeedDiamond;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(GambleTryItem), chancePercent);
            AssertHelper.NotNull(typeof(GambleTryItem), tryNeedDiamond);
        }
    }
}
