using UnityEngine;

namespace Util
{
    /// <summary>
    /// 시간 처리 관련 유틸 클래스
    /// </summary>
    public static class TimeHelper
    {
        /// <summary>
        /// 게임 일시정지 여부를 설정합니다.
        /// </summary>
        /// <param name="isPaused"></param>
        public static void SetTimePause(bool isPaused)
        {
            Time.timeScale = isPaused ? 0.0f : 1.0f;
        }
        
        /// <summary>
        /// 게임 타임 스케일을 느리게 설정합니다.
        /// </summary>
        /// <param name="isSlow"></param>
        public static void SetTimeScaleSlow(bool isSlow)
        {
            Time.timeScale = isSlow ? 0.25f : 1.0f;
        }
    }
}
