using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using TreeBuilder.Services;

namespace TreeBuilder.Components {
    public partial class IntegrationSlot : BaseItem {
        
        
        

        [Inject] public ComponentTracker cTracker { get; set; }

        public IntegrationSlot()
        {
            ClassType = CLASS_TYPE.INTEGRATIONSLOT;
        }

        public override void HandleOnDrop() {

            Console.WriteLine("IntegrationSlot:HandleOnDrop()");
            if(Payload.GetType() != typeof(Interface)) {
                Console.WriteLine(Payload.GetType().ToString());
                return;
            }

            // Hack: ondragend from Interface doesnt seem to be called when dropping an interface between two of the same items
            // Need to hide the Ghost Integration Node until I can figure out a way

            (Payload as Interface).HandleOnDragEnd();

            // End Hack

            if (Parent.Name == "GhostNode") {
                Parent.Field = cTracker.GetByName("IntegrationField") as Field;
                Field = Parent.Field;
            }

            // Remove from previous location
            if (Payload.Field != Parent.Field)
            {
                //Console.WriteLine("Fields were not : " + Payload.Parent.Field.Uid + " " + Field.Uid);
                //Payload.Parent.Items.Remove(Payload);
            }
            else
            {
                var i = -1;
                var found = false;
                foreach(var item in Payload.Parent.Items)
                {
                    i++;
                    if (item.Iface == null)
                        continue;
                    if(item.Iface.Uid == Payload.Uid)
                    {
                        found = true;
                        break;
                    }
                }
                Console.WriteLine(i);
                if (found) {
                    Payload.Parent.Items[i].Iface = null;
                    Payload.Parent.Items[i].Name = "Empty";
                }
            }

            // Add to new location and set parent and field
            if (Parent.Name != "GhostNode") {
                (Parent as IntegrationNode).Items[Index].Iface = Payload as Interface;
                (Parent as IntegrationNode).Items[Index].Name = "";
                Payload.Parent = this.Parent;
                Payload.Field = this.Field;
            }
            else {
                Console.WriteLine("IntegrationSlot:HandleOnDrop():GhostNode");
                // Create new integrationnode and populate with payload interface
                IntegrationNode inode = new IntegrationNode();
                inode.Items[Index].Iface = Payload as Interface;
                inode.Items[Index].Name = "";
                Payload.Parent = inode;
                Payload.Field = Field;
                Parent.Field.Items.Add(inode);
            }

            if (Field != null)
            {
                Field.Redraw();
            }
            CssClass = "";

            

            SaveToLocalStorageCallback.InvokeAsync();
        }

        public void DebugOutput()
        {
            Console.WriteLine($"Slot: Uid: {Uid}: Parent: {Parent.Uid}: Iface: {Iface}");
        }
    }
}