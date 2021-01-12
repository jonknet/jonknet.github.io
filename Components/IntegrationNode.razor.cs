using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System;
using Blazored.LocalStorage;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TreeBuilder.Components {
    public partial class IntegrationNode : Group {

        [Inject] ILocalStorageService LocalStorageService { get; set; }
        
        public const int MAX_IFACES = 4;

        public bool hidden = false;

        public IntegrationNode()
        {
            ClassType = CLASS_TYPE.INTEGRATIONNODE;
        }

        protected override async Task OnInitializedAsync() {
            
            if (Name == "GhostNode")
                hidden = true;
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