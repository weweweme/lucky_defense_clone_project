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
        }
        
        private void Start()
        {
            btController.Init(this);
        }

        protected override void OnDestroy()
        {
            btController.Dispose();
            
            base.OnDestroy();
        }
    }
}
