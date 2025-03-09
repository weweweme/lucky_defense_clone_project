using UnityEngine;
using Util;

namespace Unit
{
    /// <summary>
    /// 유닛의 이동 관련 움직임을 담당하는 클래스입니다.
    /// </summary>
    public sealed class UnitMoveController : MonoBehaviourBase
    {
        [SerializeField] private ParticleSystem _moveEffect;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(UnitMoveController), _moveEffect);
        }
    }
}
