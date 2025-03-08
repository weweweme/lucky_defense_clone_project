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
        
        private void Awake()
        {
            AssertHelper.NotNull(typeof(AIPlayerRoot), btController);
            
            btController.Init(this);
        }
        
        public void StartGame()
        {
            btController.StartBtTick();
        }
        
        public void StopGame()
        {
            btController.Dispose();
        }

        protected override void OnDestroy()
        {
            btController.Dispose();
            
            base.OnDestroy();
        }
    }
}
