using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TreeBuilder.Services;

namespace TreeBuilder.Components {
    
    public partial class Interface : BaseItem {
        
        [Inject] ComponentTracker cTracker { get; set; }

        public override void HandleOnDragStart(BaseItem item) {
            Payload = item;
            IntegrationNode node = (cTracker.GetByName("GhostNode") as IntegrationNode);
            node.hidden = false;
            node.Redraw();
        }

        public override void HandleOnDragEnd() {
            IntegrationNode node = (cTracker.GetByName("GhostNode") as IntegrationNode);
            node.hidden = true;
            CssClass = "";
            node.Redraw();
            for(int i = 0; i < IntegrationNode.MAX_IFACES; i++) {
                if (node.Items[i] != null) {
                    // must've dropped on the ghost node, add the node to the field
                    IntegrationField field = (cTracker.GetByName("IntegrationField") as IntegrationField);
                    var newnode = field.AddIntegrationNode();
                    newnode.Items[i] = node.Items[i];
                    // remove from the ghost node
                    node.Items[i] = null;
                    StateHasChanged();
                    return;
                }
            }
        }

        public void HandleOnDrop()
        {
            IntegrationNode node = (cTracker.GetByName("GhostNode") as IntegrationNode);
            node.hidden = true;
            node.Redraw();
        }

        public void ReleaseInterface()
        {
            IntegrationNode node = (cTracker.GetByName("GhostNode") as IntegrationNode);
            node.hidden = true;
            node.Redraw();
        }
    }
}