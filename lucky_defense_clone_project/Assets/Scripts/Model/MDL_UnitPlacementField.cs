using System;
using UniRx;

namespace Model
{
    /// <summary>
    /// UnitPlacementNode와 관련된 데이터를 관리하는 모델 클래스.
    /// </summary>
    public class MDL_UnitPlacementField
    {
        // 현재 화면에 선택된 노드와 관련된 Rx
        private readonly BehaviorSubject<UnitPlacementNode> _selectedNode = new BehaviorSubject<UnitPlacementNode>(null);
        public IObservable<UnitPlacementNode> SelectedNode => _selectedNode;
        public void SelectNode(UnitPlacementNode node) => _selectedNode.OnNext(node);
        public UnitPlacementNode GetSelectedNode() => _selectedNode.Value;
        
        // 현재 드래그 중인지 Rx
        private readonly Subject<SUnitPlacementDragData> _isDragging = new Subject<SUnitPlacementDragData>();
        public IObservable<SUnitPlacementDragData> IsDragging => _isDragging;
        public void SetIsDragging(SUnitPlacementDragData value) => _isDragging.OnNext(value);
    }
}
