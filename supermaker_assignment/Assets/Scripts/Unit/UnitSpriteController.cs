using UnityEngine;
using Util;

namespace Unit
{
    /// <summary>
    /// 유닛의 Sprite 관련 처리를 담당하는 컨트롤러 클래스입니다.
    /// </summary>
    public sealed class UnitSpriteController : MonoBehaviourBase
    {
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = gameObject.GetComponentOrAssert<SpriteRenderer>();
        }

        public void ChangeSprite()
        {
            
        }
    }
}
