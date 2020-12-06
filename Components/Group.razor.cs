using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System;


namespace TreeBuilder.Components {
    public partial class Group : Item {
     
        public List<Item> Items;

        protected override void OnInitialized()
        {
            Items = new List<Item>();
            base.OnInitialized();
        }

        public void HandleOnDrop(){
            if(Payload.Uid != this.Uid){
                Console.WriteLine("Removing item " + Payload.Uid);
                Items.Add(Payload);
                Payload.Parent.Items.Remove(Payload.Parent.Items.Find(x => x.Uid == Payload.Uid));
            }
            CssClass = "";

            this.Field.Refresh();

            Payload.Parent = this;
            
        }

        public void Debug_Output(){
            Console.WriteLine("ID: " + Uid + "\nParent: " + Parent.Uid);
            Console.WriteLine("Items: " + Items.Count);
        }

        

    }
}