using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TreeBuilder.Services;
using System;

namespace TreeBuilder.Components {
    
    public partial class Interface : BaseItem {
        
        [Inject] ComponentTracker cTracker { get; set; }
        [Inject] private IntNodeMplex nMultiplexer { get; set; }

        public Interface()
        {
            ClassType = CLASS_TYPE.INTERFACE;
        }
        protected override void OnInitialized() {
            ClassType = CLASS_TYPE.INTERFACE;
        }

        public override void HandleOnDragStart(BaseItem item) {
            Console.WriteLine("OnDragStart");
            Payload = item as Interface;
            IntegrationNode node = (cTracker.GetByName("GhostNode") as IntegrationNode);
            node.hidden = false;
            node.Redraw();
            IntegrationField field = (cTracker.GetByName("IntegrationField") as IntegrationField);
            field.ToggleExtraSlotsOn();
        }

        public override void HandleOnDragEnd() {
            Console.WriteLine("DragEnd");
            IntegrationNode node = (cTracker.GetByName("GhostNode") as IntegrationNode);
            node.hidden = true;
            CssClass = "";
            node.Redraw();
            IntegrationField field = (cTracker.GetByName("IntegrationField") as IntegrationField);
            field.ToggleExtraSlotsOff();
            SaveToLocalStorageCallback.InvokeAsync();
            /*
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
            }*/
        }

        public void DebugOutput()
        {
            Console.WriteLine($"Uid: {Uid}: ParentUid: {Parent.Uid}: Iface: {Iface}");
        }
        
    }
}