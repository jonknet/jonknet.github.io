namespace TreeBuilder.Components {
    public partial class IntegrationField : Field {

        public IntegrationNode AddIntegrationNode(){
            IntegrationNode inode = new IntegrationNode();
            inode.Field = this;
            inode.Parent = this;
            Items.Add(inode);
            Refresh();
            return inode;
        }

        public override void HandleOnDrop()
        {
            if(Payload.GetType() == typeof(Interface)){
                IntegrationNode inode = AddIntegrationNode();
                Payload.Field = this;
                Payload.Parent.Items.Remove(Payload);
                Payload.Parent = inode;
                inode.Items.Add(Payload); 
                Refresh();
            }
        }

    }
}