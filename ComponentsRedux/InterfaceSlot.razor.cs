using Microsoft.AspNetCore.Components;
using TreeBuilder.Classes;
using TreeBuilder.Components;

namespace TreeBuilder.ComponentsRedux {
    
    public enum InterfacePosition {
        Left,
        RightTop,
        RightMiddle,
        RightBottom
    }
    
    public partial class InterfaceSlot : BaseClass {
        [Parameter] public Interface Interface { get; set; } = null;
        [Parameter] public InterfacePosition Position { get; set; }
        
        public override void HandleOnDrop() {

            if (Payload.GetType() != typeof(Interface)) {
                return;
            }
            
            // Hack: ondragend from Interface doesnt seem to be called when dropping an interface between two of the same items
            // Need to hide the Ghost Integration Node until I can figure out a way
            
            (Payload as Interface).HandleOnDragEnd();

            // End Hack
            
            // Remove from previous field if switching fields
            if (Payload.Field != Field) { 
                (Payload.Parent as Group).GroupItems.Remove(Payload);
            } else if(Payload.Parent.GetType() == typeof(InterfaceSlot)){
                // Remove from previous slot by nulling it out
                ((Payload.Parent as InterfaceSlot).Parent as IntegrationNode).Interfaces[(int) this.Position] = null;
            }

            // Add to this slot
            Interface = Payload as Interface;
            Payload.Parent = this;
            Payload.Field = Field;

            CssClass = "";

            Storage.SaveToSessionStorage();
        }
    }
}