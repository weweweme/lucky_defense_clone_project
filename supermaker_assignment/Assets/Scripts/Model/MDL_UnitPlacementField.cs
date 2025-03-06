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
        private readonly Subject<UnitPlacementNode> _selectedNode = new Subject<UnitPlacementNode>();
        public IObservable<UnitPlacementNode> SelectedNode => _selectedNode;
        public void SelectNode(UnitPlacementNode node) => _selectedNode.OnNext(node);
    }
}
