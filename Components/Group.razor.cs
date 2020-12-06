using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace TreeBuilder.Components {
    public partial class Group : Item {
        List<Group> Groups;
        List<Interface> Interfaces;

        protected override void OnInitialized()
        {
            Groups = new List<Group>();
            Interfaces = new List<Interface>();
            
        }

    }
}