using System;
using TreeBuilder.Services;
using Microsoft.AspNetCore.Components;

namespace TreeBuilder.Components {
    public partial class Trash : Item {

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        public void HandleOnDrop(){
            if(Payload.GetType() == typeof(Group)){
                    foreach(Group grp in Payload.Parent.Groups){
                        if(grp.Uid == Payload.Uid){
                            Console.WriteLine("Deleting group " + grp.Uid);
                            Payload.Parent.Groups.Remove(grp);
                            break;
                        }
                    }
            } else if(Payload.GetType() == typeof(Interface)){
                    foreach(Interface iface in Payload.Parent.Interfaces){
                        if(iface.Uid == Payload.Uid){
                            Console.WriteLine("Deleting interface " + iface.Uid);
                            Payload.Parent.Interfaces.Remove(iface);
                            break;
                        }
                    }
            }
            while(Payload.Parent != null){
                Payload = Payload.Parent;
            }
            (Payload as Field).Refresh();
            Payload = null;
            
        }
    }
}