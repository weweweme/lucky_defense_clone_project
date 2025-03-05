using UnityEngine;

namespace Util
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T _ins;
    
        public static T Ins => _ins;

        private void InitSingleton()
        {
            if (_ins == null)
            {
                _ins = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    
        protected virtual void Awake()
        {
            InitSingleton();
        }
    
        private void OnDestroy()
        {
            OnDispose();
        }

        /// <summary>
        /// 인스턴스가 파괴될 때 추가로 처리할 작업을 정의하는 메서드.
        /// 여기서 싱글톤 인스턴스를 null로 설정합니다.
        /// </summary>
        protected virtual void OnDispose()
        {
            _ins = null;
        }
    }
}
