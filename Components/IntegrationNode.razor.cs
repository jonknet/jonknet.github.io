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

            if (Name == "GhostNode") {
               
                hidden = true;
                
            }

            
        }

        public void DestroyNode() {
            Parent.Items.Remove(this);
            Field.Redraw();
        }

        public void Redraw()
        {
            StateHasChanged();
        }

        /*
         public override void HandleOnDrop()
        {
            if(Payload.GetType() != typeof(Interface) || Items.Contains(Payload) || Items.Count == MAX_IFACES){
                return;
            }

            Items.Add(Payload);
            Payload.Parent.Items.Remove(Payload);
            
            Payload.Parent = this;
            while(Payload.Parent != null){
                Payload = Payload.Parent;
            }
            (Payload as Field).Refresh();
            CssClass = "";
            
        }
        */
    }
}