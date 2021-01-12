using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;


namespace TreeBuilder.Components {
    public partial class Group : BaseItem
    {

        public Group()
        {
            ClassType = CLASS_TYPE.GROUP;
        }

        public override void HandleOnDrop(){
            if (Payload.GetType() != typeof(Interface) && Payload.GetType() != typeof(Group)) {
                return;
            }
            
            // Dirty ugly hack
            if (Payload.GetType() == typeof(Interface)) {
                (Payload as Interface).HandleOnDragEnd();
            }
            // End hack
            
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
                    var i = Array.IndexOf(Payload.Parent.Items.ToArray(),Payload);
                    Payload.Parent.Items[i] = null;
                }
                else
                {
                    Payload.Parent.Items.Remove(Payload);
                }
            }
            Payload.Parent = this;

            SaveToLocalStorageCallback.InvokeAsync();

            while (Payload.Parent != null){
                Payload = Payload.Parent;
            }
            (Payload as Field).Redraw();
            CssClass = "";

            
        }

        public void Debug_Output(){
            Console.WriteLine("ID: " + Uid + "\nParent: " + Parent.Uid);
            Console.WriteLine("Items: " + Items.Count);
        }

        

    }
}