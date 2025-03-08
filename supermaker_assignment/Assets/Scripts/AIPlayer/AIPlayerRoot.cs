using System;
using AIPlayer;
using UnityEngine;
using Util;

namespace AI
{
    /// <summary>
    /// AI의 루트 클래스입니다.
    /// </summary>
    public sealed class AIPlayerRoot : MonoBehaviourBase
    {
        [SerializeField] internal AIPlayerBTController btController;
        [SerializeField] internal AIPlayerMergeController mergeController;
        internal AIPlayerDataModel dataModel;
        
        private void Awake()
        {
            AssertHelper.NotNull(typeof(AIPlayerRoot), btController);
            AssertHelper.NotNull(typeof(AIPlayerRoot), mergeController);
            
            btController.Init(this);
            mergeController.Init(this);
        }

        public void Init(RootManager rootManager)
        {
            dataModel = new AIPlayerDataModel(rootManager.DataManager);
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
