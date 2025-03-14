using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    /// <summary>
    /// 테스트 설정 패널에서 사용되는 버튼 클래스입니다
    /// </summary>
    public sealed class TestConfigPanelItem : MonoBehaviourBase
    {
        [SerializeField] internal Button selectBut;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(TestConfigPanelItem), selectBut);
        }
    }
}
