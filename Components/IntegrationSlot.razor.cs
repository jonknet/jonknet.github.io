using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using TreeBuilder.Services;

namespace TreeBuilder.Components {
    public partial class IntegrationSlot : BaseItem {
        
        [Parameter] public Interface Iface { get; set; } = null;
        [Parameter] public int Index { get; set; }

        [Inject] public ComponentTracker cTracker { get; set; }

        public void HandleOnDrop() {

            if(Payload.GetType() != typeof(Interface)){
                return;
            }

            // Hack: ondragend from Interface doesnt seem to be called when dropping an interface between two of the same items
            // Need to hide the Ghost Integration Node until I can figure out a way

            (Payload as Interface).HandleOnDragEnd();

            // End Hack

            if (Payload.Field != this.Field)
            {
                Payload.Parent.Items.Remove(Payload);
            }
            else
            {
                var i = Payload.Parent.Items.IndexOf(Payload);
                Payload.Parent.Items[i] = null;
            }
            Parent.Items[Index] = Payload;

            if (Field != null)
            {
                Field.Refresh();
            }
            CssClass = "";
        }
    }
}