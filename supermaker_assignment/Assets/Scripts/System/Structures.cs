namespace System
{
    /// <summary>
    /// 적 소환에 필요한 메타데이터 구조체입니다.
    /// </summary>
    public readonly struct SEnemySpawnRequestData
    {
        /// <summary>
        /// 소환할 적의 메타데이터 입니다.
        /// </summary>
        public EnemySpawnMetaData SpawnMetaData { get; }

        /// <summary>
        /// 적이 소환되는 진영입니다.
        /// </summary>
        public EPlayerSide SpawnSide { get; }

        /// <summary>
        /// 적 소환 메타데이터를 생성하는 생성자입니다.
        /// </summary>
        public SEnemySpawnRequestData(EnemySpawnMetaData spawnMetaData, EPlayerSide spawnSide)
        {
            SpawnMetaData = spawnMetaData;
            SpawnSide = spawnSide;
        }
    }
    
    /// <summary>
    /// 유닛 소환에 필요한 메타데이터 구조체입니다.
    /// </summary>
    public readonly struct SUnitSpawnRequestData
    {
        /// <summary>
        /// 소환할 유닛의 등급입니다.
        /// </summary>
        public EUnitGrade UnitGrade { get; }

        /// <summary>
        /// 소환할 유닛의 타입입니다.
        /// </summary>
        public EUnitType UnitType { get; }

        /// <summary>
        /// 유닛이 소환되는 진영입니다.
        /// </summary>
        public EPlayerSide SpawnSide { get; }

        /// <summary>
        /// 유닛 소환 요청 데이터를 생성하는 생성자입니다.
        /// </summary>
        /// <param name="unitGrade">소환할 유닛의 등급</param>
        /// <param name="unitType">소환할 유닛의 타입</param>
        /// <param name="spawnSide">유닛이 소환될 진영</param>
        public SUnitSpawnRequestData(EUnitGrade unitGrade, EUnitType unitType, EPlayerSide spawnSide)
        {
            UnitGrade = unitGrade;
            UnitType = unitType;
            SpawnSide = spawnSide;
        }
    }
    
    /// <summary>
    /// 유닛 배치 노드의 드래그 상태와 타겟 노드 정보를 담는 구조체입니다.
    /// </summary>
    public readonly struct SUnitPlacementDragData
    {
        /// <summary>
        /// 현재 드래그 상태 여부입니다.
        /// </summary>
        public bool IsDragging { get; }

        /// <summary>
        /// 드래그 중 마우스 위치에 가장 가까운 유닛 배치 노드입니다.
        /// </summary>
        public UnitPlacementNode TargetNode { get; }

        /// <summary>
        /// 유닛 배치 드래그 데이터를 생성하는 생성자입니다.
        /// </summary>
        /// <param name="isDragging">드래그 상태 여부</param>
        /// <param name="targetNode">드래그 타겟 노드</param>
        public SUnitPlacementDragData(bool isDragging, UnitPlacementNode targetNode)
        {
            IsDragging = isDragging;
            TargetNode = targetNode;
        }
    }
}
