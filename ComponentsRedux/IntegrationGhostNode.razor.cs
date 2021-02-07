using TreeBuilder.Classes;

namespace TreeBuilder.ComponentsRedux {
    /// <summary>
    ///     IntegrationNode that appears only when dragging an interface to provide ability to create new Nodes
    /// </summary>
    public partial class IntegrationGhostNode : IntegrationNode
    {
        public void HandleOnDrop(InterfaceSlotPosition Position)
        {
            var Node = new IntegrationNode();
            Node.Parent = Storage.IntegrationField;
            Node.Field = Storage.IntegrationField;
            
            BaseClass b = EventState.FindItem(EventState.Payload.Guid);
            
            EventState.DeleteItem(EventState.Payload.Guid.ToString());
            
            b.Parent = new InterfaceSlot();


            if (Position == InterfaceSlotPosition.Left)
            {
                Node.Interfaces[(int) InterfaceSlotPosition.Left] = b as Interface;
                (b.Parent as InterfaceSlot).Position = (int) InterfaceSlotPosition.Left;
            }
            else if (Position == InterfaceSlotPosition.Right)
            {
                Node.Interfaces[(int) InterfaceSlotPosition.Right] = b as Interface;
                (b.Parent as InterfaceSlot).Position = (int) InterfaceSlotPosition.Right;
            }
            
            b.Parent.Parent = Node;
            b.Field = Storage.IntegrationField;



            Storage.IntegrationField.GroupItems.Add(Node);

            base.HandleOnDragEnd();
            
            RenderService.Redraw();
            Storage.SaveToSessionStorage();
        }
    }
}