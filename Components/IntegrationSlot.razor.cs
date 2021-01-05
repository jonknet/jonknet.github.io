using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace TreeBuilder.Components {
    public partial class IntegrationSlot : BaseItem {
        
        [Parameter] public Interface Iface { get; set; } = null;
        [Parameter] public int Index { get; set; }

        public void HandleOnDrop() {
            if(Payload.GetType() != typeof(Interface)){
                return;
            }

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
            
            Field.Refresh();
            CssClass = "";
        }
    }
}