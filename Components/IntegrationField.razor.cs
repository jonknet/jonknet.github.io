namespace TreeBuilder.Components {
    public partial class IntegrationField : Field {

        public void AddIntegrationNode(){
            IntegrationNode inode = new IntegrationNode();
            inode.Parent = this;
            Items.Add(inode);
            Refresh();
        }

    }
}