using UnityEngine;
using Util;

namespace Enemy
{
    /// <summary>
    /// 에너미의 Sprite 및 시각적 요소 처리를 담당하는 컨트롤러 클래스입니다.
    /// </summary>
    public sealed class EnemySpriteController : MonoBehaviourBase
    { 
        /// <summary>
        /// 에너미의 스프라이트 렌더링을 담당하는 컴포넌트
        /// </summary>
		[SerializeField] private SpriteRenderer _spriteRenderer;

        private EnemyRoot _enemyRoot;
        
        private void Awake()
        {
            AssertHelper.NotNull(typeof(EnemySpriteController), _spriteRenderer);
        }

        /// <summary>
        /// 에너미 생성 시점에 EnemyRoot 정보를 주입받아 내부 참조를 설정합니다.
        /// </summary>
        /// <param name="root">해당 에너미의 Enemy 참조</param>
        public void CreatePooledItemInit(EnemyRoot root)
        {
            _enemyRoot = root;
        }

        /// <summary>
        /// 애너미의 등급과 타입에 맞는 스프라이트 및 크기 정보를 조회하여 실제 적용합니다.
        /// 에너미 외형을 현재 상태에 맞게 갱신하는 역할을 합니다.
        /// </summary>
        public void ChangeVisible()
        {
            EnemyMetaData metaData = _enemyRoot.dependencyContainer.mdlEnemyResources.GetResources(_enemyRoot.currentSpawnWaveIdx);
            AssertHelper.NotNull(typeof(EnemySpriteController), metaData);
            
            _spriteRenderer.sprite = metaData.Sprite;
        }
    }
}
