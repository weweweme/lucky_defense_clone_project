using InGame.System;
using TMPro;
using UnityEngine;
using Util;

namespace UI
{
    /// <summary>
    /// 재화와 관련된 UI를 관리하는 View 클래스입니다.
    /// </summary>
    public sealed class VW_Currency : View
    {
        [SerializeField] private TextMeshProUGUI _currency;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(VW_Currency), _currency);
        }

        /// <summary>
        /// 재화를 업데이트합니다.
        /// </summary>
        /// <param name="currency"></param>
        public void UpdateCurrency(uint currency)
        {
            _currency.SetText(currency.ToString());
        }
    }
}
