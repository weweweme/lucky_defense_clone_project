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
        private readonly MDL_Wave _wave = new MDL_Wave();
        public MDL_Wave Wave => _wave;

        private readonly MDL_Enemy _enemy = new MDL_Enemy();
        public MDL_Enemy Enemy => _enemy;
        
        private readonly MDL_GameSystem _gameSystem = new MDL_GameSystem();
        public MDL_GameSystem GameSystem => _gameSystem;
        
        private readonly MDL_Unit _unit = new MDL_Unit();
        public MDL_Unit Unit => _unit;
        
        private readonly MDL_Currency _currency = new MDL_Currency();
        public MDL_Currency Currency => _currency;
        
        [SerializeField] private MDL_UnitResources _unitResources;
        public MDL_UnitResources UnitResources => _unitResources;

        private void Awake()
        {
            AssertHelper.NotNull(typeof(DataManager), _unitResources);
        }
    }
}
