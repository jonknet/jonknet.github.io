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

            Item index;
            for(index = Payload.Parent; index.GetType() != typeof(Field); index = index.Parent){}
            (index as Field).Refresh();
            CssClass = "";
            Payload.Parent = this;
        }
    }
}