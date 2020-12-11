using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System;
using TreeBuilder.Services;


namespace TreeBuilder.Components {
    public partial class Field : Group {

        [Parameter]
        public string Name { get; set; } = null;

        public void AddGroup(){
            Group grp = new Group();
            grp.Parent = this;
            Items.Add(grp);
            Refresh();
        }

        public void AddInterface(){
            Interface iface = new Interface();
            iface.Parent = this;
            Items.Add(iface);
            Refresh();
        }

        public void Refresh(){
            StateHasChanged();
        }

    }
}