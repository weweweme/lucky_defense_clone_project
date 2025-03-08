using AI;
using UnityEngine;
using Util;

namespace System
{
    /// <summary>
    /// 게임의 시퀀스를 관리하는 매니저 클래스입니다.
    /// </summary>
    public sealed class SequenceManager : MonoBehaviourBase
    {
        [SerializeField] private AIPlayerRoot _aiPlayerRoot;
        
        private void Awake()
        {
            AssertHelper.NotNull(typeof(SequenceManager), _aiPlayerRoot);
        }
        
        public void StartGame()
        {
            _aiPlayerRoot.ActivateAI();
        }

        public void StopGame()
        {
            _aiPlayerRoot.DeactivateAI();
        }
    }
}
