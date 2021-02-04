using Microsoft.AspNetCore.Components;
using TreeBuilder.Classes;

namespace TreeBuilder.ComponentsRedux {
    public partial class InterfaceSlot : BaseClass {
        [Parameter] public int Position { get; set; }
        [Parameter] public string DomId { get; set; }
        public override void HandleOnDrop() {
            if (!Is<Interface>(EventState.Payload)) return;

            // Hack: ondragend from Interface doesnt seem to be called when dropping an interface between two of the same items
            // Need to hide the Ghost Integration Node until I can figure out a way

            EventState.Payload.HandleOnDragEnd();

            // End Hack

            // Remove from previous field if switching fields
            if (EventState.Payload.Field != Field) {
                EventState.Payload.Parent.GroupItems.Remove(EventState.Payload);
            }
            else if (Is<InterfaceSlot>(EventState.Payload.Parent)) {
                // Remove from previous slot by nulling it out
                var interfaces = (EventState.Payload.Parent.Parent as IntegrationNode).Interfaces;
                for (var i = 0; i < interfaces.Length; i++)
                    if (interfaces[i] != null && interfaces[i].Equals(EventState.Payload)) {
                        interfaces[i] = null;
                        break;
                    }
            }

            // Add to this slot

            (Parent as IntegrationNode).Interfaces[Position] = EventState.Payload as Interface;
            EventState.Payload.Parent = this;
            EventState.Payload.Field = Field;

            CssClass = "";

            RenderService.Redraw();

            Storage.SaveToSessionStorage();
        }
    }
}