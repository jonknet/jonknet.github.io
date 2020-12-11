using System;
using TreeBuilder.Services;
using Microsoft.AspNetCore.Components;

namespace TreeBuilder.Components {
    public partial class Trash : Item {

        protected override void OnInitialized()
        {
            base.OnInitialized();
            //
        }

        public void HandleOnDrop(){
            Payload.Parent.Items.Remove(Payload);
            while(Payload.Parent != null){
                Payload = Payload.Parent;
            }
            (Payload as Field).Refresh();
        }
    }
}