using System.Collections.Generic;
using Enemy;
using UnityEditor.Animations;
using UnityEngine;
using Util;

namespace Model
{
    /// <summary>
    /// 프로젝트 내부에서 사용되는 적 캐릭터의 리소스를 관리하는 클래스입니다.
    /// </summary>
    public sealed class MDL_EnemyResources : MonoBehaviourBase
    {
        private const uint CURRENT_ENEMY_COUNT = 10;
        [SerializeField] private List<EnemyMetaData> _enemyResourceList;
        [SerializeField] private AnimatorController _enemyDeathAnimationController;
        
        private void Awake()
        {
            AssertHelper.EqualsValue(typeof(MDL_EnemyResources), (uint)_enemyResourceList.Count, CURRENT_ENEMY_COUNT);
        }
        
        public EnemyMetaData GetResources(uint waveIdx)
        {
            uint targetIdx = (waveIdx - 1) % CURRENT_ENEMY_COUNT;
            return _enemyResourceList[(int)targetIdx];
        }
        
        public AnimatorController GetDeathAnimationController() => _enemyDeathAnimationController;
    }
}
