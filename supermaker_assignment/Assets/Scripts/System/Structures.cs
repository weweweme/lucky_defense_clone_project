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

    /// <summary>
    /// 특정 유닛 조합 조건을 정의하는 구조체입니다.
    /// 유닛 합성 등의 조건 체크 시, 요구되는 유닛 등급과 타입을 명시하는 데 사용됩니다.
    /// </summary>
    public readonly struct SUnitCombinationFlagCondition
    {
        /// <summary>
        /// 해당 조합 조건에서 요구하는 유닛 등급입니다.
        /// </summary>
        public EUnitGrade Grade { get; }

        /// <summary>
        /// 해당 조합 조건에서 요구하는 유닛 타입입니다.
        /// </summary>
        public EUnitType Type { get; }
        
        /// <summary>
        /// 유닛 조합 조건을 정의하는 생성자입니다.
        /// 원하는 유닛 등급과 타입을 지정하여 조건을 구성합니다.
        /// </summary>
        /// <param name="grade">조합 조건에 필요한 유닛 등급</param>
        /// <param name="type">조합 조건에 필요한 유닛 타입</param>
        public SUnitCombinationFlagCondition(EUnitGrade grade, EUnitType type)
        {
            Grade = grade;
            Type = type;
        }
    }
    
    /// <summary>
    /// 현재 보여 줄 신화 유닛 조합 정보를 담는 구조체입니다.
    /// 신화 유닛 조합 패널 등에서 현재 어떤 유닛 조합을 진행 중인지 표시할 때 사용됩니다.
    /// </summary>
    public readonly struct SCurrentMythicUnitCombinationData
    {
        /// <summary>
        /// 신화 유닛의 이름입니다.
        /// </summary>
        public string UnitName { get; }

        /// <summary>
        /// 신화 유닛의 타입입니다.
        /// </summary>
        public EUnitType UnitType { get; }

        /// <summary>
        /// 현재 진행 중인 신화 유닛 조합 정보를 생성하는 생성자입니다.
        /// </summary>
        /// <param name="unitName">신화 유닛 이름</param>
        /// <param name="unitType">신화 유닛 타입</param>
        public SCurrentMythicUnitCombinationData(string unitName, EUnitType unitType)
        {
            UnitName = unitName;
            UnitType = unitType;
        }
    }

    /// <summary>
    /// 특정 유닛 등급에 대한 도박(뽑기) 정보를 저장하는 구조체입니다.
    /// 도박 시 성공 확률과 필요한 다이아 수량을 포함합니다.
    /// </summary>
    public readonly struct SGambleMetaData
    {
        /// <summary>
        /// 도박 대상 유닛의 등급입니다.
        /// </summary>
        public EUnitGrade Grade { get; }

        /// <summary>
        /// 도박 성공 확률 (0.0f ~ 1.0f 범위).
        /// </summary>
        public float SuccessProbability { get; }

        /// <summary>
        /// 도박 시 필요한 다이아 수량입니다.
        /// </summary>
        public uint RequiredDia { get; }

        /// <summary>
        /// 도박 메타데이터를 초기화하는 생성자입니다.
        /// </summary>
        /// <param name="grade">도박 대상 유닛 등급</param>
        /// <param name="successProbability">도박 성공 확률 (0.0f ~ 1.0f)</param>
        /// <param name="requiredDia">필요한 다이아 수량</param>
        public SGambleMetaData(EUnitGrade grade, float successProbability, uint requiredDia)
        {
            Grade = grade;
            SuccessProbability = successProbability;
            RequiredDia = requiredDia;
        }
    }
}
