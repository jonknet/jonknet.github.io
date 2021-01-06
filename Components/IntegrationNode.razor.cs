using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System;

namespace TreeBuilder.Components {
    public partial class IntegrationNode : Group {
        
        public const int MAX_IFACES = 4;

        public bool hidden = false;

        protected override void OnInitialized() {
            Items.Add(null);
            Items.Add(null);
            Items.Add(null);
            Items.Add(null);
            if (Name == "GhostNode")
                hidden = true;
        }

        public void DestroyNode() {
            Parent.Items.Remove(this);
            Field.Refresh();
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