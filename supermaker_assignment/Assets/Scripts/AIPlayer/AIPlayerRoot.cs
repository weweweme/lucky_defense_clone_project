using System;
using UnityEngine;
using Util;

namespace AIPlayer
{
    /// <summary>
    /// AI의 루트 클래스입니다.
    /// </summary>
    public sealed class AIPlayerRoot : MonoBehaviourBase
    {
        [SerializeField] internal AIPlayerBTController btController;
        [SerializeField] internal AIPlayerMergeController mergeController;
        [SerializeField] internal AIPlayerSpawnController spawnController;
        [SerializeField] internal AIPlayerGambleController gambleController;
        [SerializeField] internal AIPlayerMythicUnitCombinationController mythicUnitCombinationController;
        internal AIPlayerDataModel dataModel;
        internal RootManager globalRootManager;
        
        private void Awake()
        {
            AssertHelper.NotNull(typeof(AIPlayerRoot), btController);
            AssertHelper.NotNull(typeof(AIPlayerRoot), mergeController);
            AssertHelper.NotNull(typeof(AIPlayerRoot), spawnController);
            AssertHelper.NotNull(typeof(AIPlayerRoot), gambleController);
            AssertHelper.NotNull(typeof(AIPlayerRoot), mythicUnitCombinationController);
        }

        public void Init(RootManager rootManager)
        {
            dataModel = new AIPlayerDataModel(rootManager.DataManager);
            AssertHelper.NotNull(typeof(AIPlayerRoot), dataModel);
            
            globalRootManager = rootManager;
            AssertHelper.NotNull(typeof(AIPlayerRoot), this.globalRootManager);
            
            btController.Init(this);
            mergeController.Init(this);
            spawnController.Init(this);
            gambleController.Init(this);
            mythicUnitCombinationController.Init(this);
        }
        
        public void ActivateAI()
        {
            btController.StartBtTick();
        }
        
        public void DeactivateAI()
        {
            btController.Dispose();
        }

        protected override void OnDestroy()
        {
            btController.Dispose();
            dataModel.Dispose();
            
            base.OnDestroy();
        }
    }
}
