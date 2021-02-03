using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using TreeBuilder.Services;

namespace TreeBuilder.Components {
    public partial class IntegrationField : Field
    {
        [JsonIgnore] public Dictionary<Guid, IntegrationNode> intNodeRefs = new Dictionary<Guid, IntegrationNode>();
        public IntegrationField()
        {
            ClassType = CLASS_TYPE.INTEGRATIONFIELD;
        }

        [Inject] private ILocalStorageService LocalStorageService { get; set; }
        protected override async Task OnInitializedAsync()
        {
            if (await LocalStorageService.ContainKeyAsync("TreeBuilder_IntegrationField")) {
                IntegrationField _if = JsonConvert.DeserializeObject<IntegrationField>(await LocalStorageService.GetItemAsStringAsync("TreeBuilder_IntegrationField"), new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.All,
                    NullValueHandling = NullValueHandling.Ignore
                });
            Console.WriteLine(_if);
            
            Items = _if.Items;
            Title = _if.Title;
            Uid = _if.Uid;
            

            }
        }

        public IntegrationNode AddIntegrationNode(){
            IntegrationNode inode = new IntegrationNode();
            // Redraw();
            Items.Add(inode);
            return inode;
        }

        public void ToggleExtraSlotsOn() {
            foreach (var node in intNodeRefs.Values) {
                if (node.Items[2].Iface != null) {
                    node.showExtraSlots = true;
                    node.Redraw();
                }
            }
        }
        
        public void ToggleExtraSlotsOff() {
            foreach (var node in intNodeRefs.Values) {
                if (node.Items[2].Iface != null) {
                    node.showExtraSlots = false;
                    node.Redraw();
                }
            }
        }

        public async void SaveSession()
        {
            await LocalStorageService.SetItemAsync("TreeBuilder_IntegrationField", this as BaseItem);
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

            } else if (Payload.GetType() == typeof(IntegrationNode)) {
                if (Items.Contains(Payload)) {
                    return;
                }
                Items.Add(Payload);
                Payload.Parent.Nodes.Remove(Payload as IntegrationNode);
                Payload.Parent = this;
                Payload.Field = this;
            }
            SaveToLocalStorageCallback.InvokeAsync();
        }

    }
}