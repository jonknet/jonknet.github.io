using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System;


namespace TreeBuilder.Components {
    public partial class Group : Item {
        public List<Group> Groups = new List<Group>();
        public List<Interface> Interfaces = new List<Interface>();

        [Parameter]
        public RenderFragment ChildContent {get; set;}

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        public void HandleOnDrop(){
            if(Payload != null && Payload.Uid != this.Uid){
                if(Payload.GetType() == typeof(Group)){
                    Groups.Add(Payload as Group);
                    foreach(Group grp in Payload.Parent.Groups){
                        if(grp.Uid == Payload.Uid){
                            Payload.Parent.Groups.Remove(grp);
                            break;
                        }
                    }
                } else if(Payload.GetType() == typeof(Interface)){
                    Interfaces.Add(Payload as Interface);
                    foreach(Interface iface in Payload.Parent.Interfaces){
                        if(iface.Uid == Payload.Uid){
                            Payload.Parent.Interfaces.Remove(iface);
                            break;
                        }
                    }
                }
                
            }
            CssClass = "";

            this.Field.Refresh();

            Payload.Parent = this;

            Payload = null;

            
        }

        

    }
}