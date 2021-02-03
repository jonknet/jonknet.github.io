using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System;
using Blazored.LocalStorage;
using System.Threading.Tasks;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;
using Newtonsoft.Json;
using TreeBuilder.Services;

namespace TreeBuilder.Components {
    public partial class IntegrationNode : Group {

        [Inject] ILocalStorageService LocalStorageService { get; set; }
        [Inject] public ComponentTracker cTracker { get; set; }

        
        
        public const int MAX_IFACES = 4;

        public bool hidden = false;

        public bool showExtraSlots = false;

        public IntegrationNode() {
            ClassType = CLASS_TYPE.INTEGRATIONNODE;
            for(int i = 0; i < 4; i++)
            {
                IntegrationSlot iface = new IntegrationSlot();
                iface.Name = "Empty";
                iface.Index = i;
                iface.Field = this.Field;
                iface.Parent = this;
                Items.Add(iface);
            }
            
            

        }

        protected override async Task OnInitializedAsync() {
            ClassType = CLASS_TYPE.INTEGRATIONNODE;
            if (Name == "GhostNode") {
                hidden = true;
            }
        }

        public void DestroyNode() {
            Parent.Items.Remove(this);
            (Field as IntegrationField).intNodeRefs.Remove(this.Uid);
            Field.Redraw();
        }

        public void Redraw()
        {
            StateHasChanged();
        }

        public void ToggleExtraSlots() {
            foreach (var node in (Field as IntegrationField).intNodeRefs.Values) {
                if (node.Items[2].Iface != null) {
                    node.showExtraSlots = !node.showExtraSlots;
                    node.Redraw();
                }
            }

        }

        
        public override void HandleOnDrop()
        {
            if(Payload.GetType() != typeof(IntegrationNode) || Payload == this || Payload.Parent == this){
                return;
            }

            if (Payload.Parent.Nodes.Contains(Payload as IntegrationNode)) {
                Console.WriteLine("Removing from IntNode " + Payload.Uid);
                Payload.Parent.Nodes.Remove(Payload as IntegrationNode);
            } else {
                Console.WriteLine("Removing from Field " + Payload.Uid);
                Payload.Parent.Items.Remove(Payload);
            }
            
            Nodes.Add(Payload as IntegrationNode);
            Payload.Parent = this;



            SaveToLocalStorageCallback.InvokeAsync();
        }
        
    }
}