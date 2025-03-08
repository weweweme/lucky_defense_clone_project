using Model;
using UnityEngine;
using Util;

namespace System
{
    /// <summary>
    /// 게임 내에서 사용될 모델들의 참조와 데이터를 관리할 DataManager 클래스
    /// </summary>
    public sealed class DataManager : MonoBehaviourBase
    {
        public MDL_Wave Wave { get; } = new MDL_Wave();
        public MDL_Enemy Enemy { get; } = new MDL_Enemy();
        public MDL_GameSystem GameSystem { get; } = new MDL_GameSystem();
        public MDL_Unit Unit { get; } = new MDL_Unit();
        public MDL_Currency Currency { get; } = new MDL_Currency();
        public MDL_MythicUnitCombination MythicUnitCombination { get; } = new MDL_MythicUnitCombination();

        [SerializeField] private MDL_UnitResources _unitResources;
        public MDL_UnitResources UnitResources => _unitResources;
        
        [SerializeField] private MDL_EnemyResources _enemyResources;
        public MDL_EnemyResources EnemyResources => _enemyResources;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(DataManager), _unitResources);
            AssertHelper.NotNull(typeof(DataManager), _enemyResources);
        }
    }
}
