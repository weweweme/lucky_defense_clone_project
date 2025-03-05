using Model;
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
    }
}
