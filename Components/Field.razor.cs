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
            Groups.Add(grp);
            StateHasChanged();
        }

        public void AddInterface(){
            Interface iface = new Interface();
            iface.Parent = this;
            Interfaces.Add(iface);
            StateHasChanged();
        }

        public void Refresh(){
            StateHasChanged();
        }

    }
}