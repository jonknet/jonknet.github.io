using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System;


namespace TreeBuilder.Components {
    public partial class Group : BaseItem {

        [Parameter] public List<BaseItem> Items { get; set; } = new List<BaseItem>();

        protected override void OnInitialized() {
            base.OnInitialized();
        }

        public virtual void HandleOnDrop(){
            if(Payload != this){
                //If dragging an object from the Field to the same Field
                if(Payload.Parent == this){
                    return;
                }
                Console.WriteLine("Adding item " + Payload.Uid + " to " + this.Uid);
                Console.WriteLine("Removing item " + Payload.Uid + " from " + Payload.Parent.Uid);
                Items.Add(Payload);
                if (Payload.Field != this.Field)
                {
                    var i = Payload.Parent.Items.IndexOf(Payload);
                    Payload.Parent.Items[i] = null;
                }
                else
                {
                    Payload.Parent.Items.Remove(Payload);
                }
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