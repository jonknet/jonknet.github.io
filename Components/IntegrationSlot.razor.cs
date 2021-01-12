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

            if(Payload.ClassType != CLASS_TYPE.INTERFACE){
                return;
            }

            // Hack: ondragend from Interface doesnt seem to be called when dropping an interface between two of the same items
            // Need to hide the Ghost Integration Node until I can figure out a way

            (Payload as Interface).HandleOnDragEnd();

            // End Hack

            if (Payload.Parent.Field != Parent.Field)
            {
                Console.WriteLine("Fields were not equal");
                Payload.Parent.Items.Remove(Payload);
            }
            else
            {
                var i = -1;
                foreach(var item in Payload.Parent.Items)
                {
                    i++;
                    if (item.Iface == null)
                        continue;
                    if(item.Iface.Uid == Payload.Uid)
                    {
                        break;
                    }
                }
                Console.WriteLine(i);
                Payload.Parent.Items[i].Iface = null;
                Payload.Parent.Items[i].Name = "Empty";

            }

            
            (Parent as IntegrationNode).Items[Index].Iface = Payload as Interface;
            (Parent as IntegrationNode).Items[Index].Name = "";
            Payload.Parent = this.Parent;
            
            
            Payload.Field = this.Field;

            if (Field != null)
            {
                Field.Redraw();
            }
            CssClass = "";

            Payload = null;

            SaveToLocalStorageCallback.InvokeAsync();
        }

        public void DebugOutput()
        {
            Console.WriteLine($"Slot: Uid: {Uid}: Parent: {Parent.Uid}: Iface: {Iface}");
        }
    }
}