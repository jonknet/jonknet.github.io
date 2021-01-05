using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System;

namespace TreeBuilder.Components {
    public partial class IntegrationNode : Group {
        
        
        public const int MAX_IFACES = 4;

        protected override void OnInitialized() {
            for(int i = 0; i < MAX_IFACES; i++)
            {
                Items.Add(null);
            }
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