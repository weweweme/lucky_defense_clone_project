using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    /// <summary>
    /// 신화 유닛 조합 패널에서 각 필요 유닛 정보를 표시하는 슬롯 클래스입니다.
    /// </summary>
    public sealed class RequiredUnitSlot : MonoBehaviourBase
    {
        /// <summary>
        /// 필요 유닛의 아이콘 이미지입니다.
        /// </summary>
        [SerializeField] internal Image unitIcon;

        /// <summary>
        /// 해당 유닛이 아직 조건 미충족일 때 표시되는 패널입니다.
        /// </summary>
        [SerializeField] internal GameObject deniedPanel;

        /// <summary>
        /// 필수 컴포넌트들의 참조 여부를 확인하는 초기화 메서드입니다.
        /// </summary>
        private void Awake()
        {
            AssertHelper.NotNull(typeof(RequiredUnitSlot), unitIcon);
            AssertHelper.NotNull(typeof(RequiredUnitSlot), deniedPanel);
        }
    }
}
