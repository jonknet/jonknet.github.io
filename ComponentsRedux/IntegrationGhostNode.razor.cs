using TreeBuilder.Classes;

namespace TreeBuilder.ComponentsRedux {
    /// <summary>
    ///     IntegrationNode that appears only when dragging an interface to provide ability to create new Nodes
    /// </summary>
    public partial class IntegrationGhostNode : IntegrationNode {
        public void HandleOnDrop(InterfaceSlotPosition Position) {
            var Node = new IntegrationNode();
            Node.Parent = Storage.IntegrationField;
            Node.Field = Storage.IntegrationField;
            if (Position == InterfaceSlotPosition.Left)
                Node.Interfaces[(int) InterfaceSlotPosition.Left] = EventState.Payload as Interface;
            else if (Position == InterfaceSlotPosition.Right)
                Node.Interfaces[(int) InterfaceSlotPosition.Right] = EventState.Payload as Interface;
            Storage.IntegrationField.IntegrationNodes.Add(Node);
            RenderService.Redraw();
            Storage.SaveToSessionStorage();
        }
    }
}