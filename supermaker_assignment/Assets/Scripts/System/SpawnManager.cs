namespace System
{
    /// <summary>
    /// 스폰과 관련된 모듈의 관리하는 매니저 클래스입니다.
    /// </summary>
    public sealed class SpawnManager : IDisposable
    {
        private readonly EnemySpawnHandler _enemySpawnHandler;
        
        public SpawnManager(RootManager rootManager)
        {
            _enemySpawnHandler = new EnemySpawnHandler(rootManager);
        }

        public void Dispose()
        {
            _enemySpawnHandler.Dispose();
        }
    }
}
