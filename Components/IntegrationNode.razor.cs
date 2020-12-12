using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System;

namespace TreeBuilder.Components {
    public partial class IntegrationNode : Group {
        
        [Parameter]
        public static int MAX_IFACES { get; set; } = 4;

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
    }
}