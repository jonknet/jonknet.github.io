using System.Collections.Generic;
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
            Node.GroupItems = new List<BaseClass>();
            
            BaseClass b = new Interface();
            b.Guid = System.Guid.NewGuid();
            b.Parent = Node;
            b.Field = Storage.IntegrationField;
            
            Node.Interfaces[(int) Position] = b as Interface;
            
            Storage.IntegrationField.GroupItems.Add(Node);

            Storage.SaveToSessionStorage();
            
            base.HandleOnDragEnd();
        }
    }
}