using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System;
using TreeBuilder.Services;


namespace TreeBuilder.Components {
    public partial class Field : Group {
        
        protected override void OnInitialized()
        {
            base.OnInitialized();
            
        }

        public void AddGroup(){
            Group grp = new Group();
            grp.Parent = this;
            Items.Add(grp);
            StateHasChanged();
        }

        public void AddInterface(){
            Interface iface = new Interface();
            iface.Parent = this;
            Items.Add(iface);
            StateHasChanged();
        }

        public void Refresh(){
            StateHasChanged();
        }

    }
}