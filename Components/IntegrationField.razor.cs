using System;

namespace TreeBuilder.Components {
    public partial class IntegrationField : Field {
        public IntegrationNode AddIntegrationNode(){
            IntegrationNode inode = new IntegrationNode();
            inode.Items.Add(null);
            inode.Items.Add(null);
            inode.Items.Add(null);
            inode.Items.Add(null);
            inode.Field = this;
            inode.Parent = this;
            Items.Add(inode);
            Refresh();
            return inode;
        }
        public override void HandleOnDrop()
        {
            if(Payload.GetType() == typeof(Interface)){
                
                // Dirty Hack to allow the ghost Integration Node to hide
                (Payload as Interface).HandleOnDragEnd();
                // End Hack
                
                IntegrationNode inode = AddIntegrationNode();
                if (Payload.Field.GetType() != typeof(IntegrationField)) {
                    // Remove
                    Payload.Parent.Items.Remove(Payload);
                }
                else {
                    var i = Array.IndexOf(Payload.Parent.Items.ToArray(), Payload);
                    Payload.Parent.Items[i] = null;
                }
                Payload.Field = this;
                inode.Items[0] = Payload;
                Payload.Parent = inode;
                
                base.StateHasChanged();
            }
        }

    }
}