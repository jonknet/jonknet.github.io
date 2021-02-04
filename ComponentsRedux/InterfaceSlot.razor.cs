using Microsoft.AspNetCore.Components;
using TreeBuilder.Classes;
using TreeBuilder.Components;

namespace TreeBuilder.ComponentsRedux {

    public partial class InterfaceSlot : BaseClass {
        
        [Parameter] public int Position { get; set; }
        

        protected override void OnInitialized() {
            
        }

        public override void HandleOnDrop() {

            if (!Is<Interface>(Payload)) {
                return;
            }
            
            // Hack: ondragend from Interface doesnt seem to be called when dropping an interface between two of the same items
            // Need to hide the Ghost Integration Node until I can figure out a way
            
            //(Payload as Interface).HandleOnDragEnd();

            // End Hack
            
            // Remove from previous field if switching fields
            if (Payload.Field != this.Field) { 
                Payload.Parent.GroupItems.Remove(Payload);
            } else if(Is<InterfaceSlot>(Payload.Parent)){
                // Remove from previous slot by nulling it out
                Interface[] interfaces = (Payload.Parent.Parent as IntegrationNode).Interfaces;
                for (int i = 0; i < interfaces.Length; i++) {
                    if (interfaces[i] != null && interfaces[i].Equals(Payload)) {
                        interfaces[i] = null;
                        break;
                    }
                }
            }

            // Add to this slot
            
            (Parent as IntegrationNode).Interfaces[Position] = Payload as Interface;
            Payload.Parent = this;
            Payload.Field = Field;

            CssClass = "";
            
            RenderService.Redraw();

            Storage.SaveToSessionStorage();
        }
    }
}