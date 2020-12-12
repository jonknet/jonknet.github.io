using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System;


namespace TreeBuilder.Components {
    public partial class Group : Item {
     
        [Parameter]
        public List<Item> Items { get; set; } = new List<Item>();

        public virtual void HandleOnDrop(){
            if(Payload != this){
                //If dragging an object from the Field to the same Field
                if(Payload.Parent == this){
                    return;
                }
                Console.WriteLine("Adding item " + Payload.Uid + " to " + this.Uid);
                Console.WriteLine("Removing item " + Payload.Uid + " from " + Payload.Parent.Uid);
                Items.Add(Payload);
                Payload.Parent.Items.Remove(Payload);
            }
            Payload.Parent = this;
            while(Payload.Parent != null){
                Payload = Payload.Parent;
            }
            (Payload as Field).Refresh();
            CssClass = "";
        }

        public void Debug_Output(){
            Console.WriteLine("ID: " + Uid + "\nParent: " + Parent.Uid);
            Console.WriteLine("Items: " + Items.Count);
        }

        

    }
}