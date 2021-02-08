using System;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Components;
using TreeBuilder.Classes;

namespace TreeBuilder.ComponentsRedux {
    public partial class InterfaceSlot : BaseClass {
        [Parameter] public int Position { get; set; }
        [Parameter] public string DomId { get; set; }

        public override void HandleOnDrop() {
            if (!Is<Interface>(EventState.Payload)) return;

            BaseClass b = null;

            // Create new interface if transferring fields
            if (EventState.Payload.Field.Is<GroupField>())
            {
                b = new Interface();
                b.Title = EventState.Payload.Title;
                b.Guid = Guid.NewGuid();
            }
            else if (Is<IntegrationNode>(EventState.Payload.Parent))
            {
                b = EventState.FindItem(EventState.Payload.Guid);
                var Inode = EventState.FindItem(EventState.Payload.Parent.Guid);
                
                // Remove from previous slot by nulling it out
                var interfaces = (Inode as IntegrationNode).Interfaces;
                for (var i = 0; i < interfaces.Length; i++)
                    if (interfaces[i] != null && interfaces[i].Equals(EventState.Payload)) {
                        interfaces[i] = null;
                        break;
                    }
            }
            
            b.Parent = Parent;
            b.Field = Field;

            // Add to this slot

            (Parent as IntegrationNode).Interfaces[Position] = b as Interface;

            Storage.SaveToSessionStorage();
            
            // Make sure to end drag event so ghost node goes away
            base.HandleOnDragEnd();
        }
    }
}