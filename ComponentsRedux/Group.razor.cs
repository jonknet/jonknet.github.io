using System;
using TreeBuilder.Classes;

namespace TreeBuilder.ComponentsRedux {
    public partial class Group : BaseClass {
        public Group() { }
        public Group(BaseClass Parent) : base(Parent, Parent as Group) { }
        

        public override void HandleOnDrop()
        {
            if ((!EventState.Payload.Is<Interface>() && !EventState.Payload.Is<Group>()) ||
                GroupItems.Contains(EventState.Payload) ||
                EventState.Payload == this)
            {
                return;
            }
            
            EventState.DeleteItem(EventState.Payload.Guid.ToString());

            EventState.Payload.Parent = this;
            
            GroupItems.Add(EventState.Payload);

            if (Is<GroupField>())
            {
                EventState.Payload.Field = this;
            }
            else
            {
                EventState.Payload.Field = Field;
            }
            
            CssClass = "";
            EventState.DraggingEvent = false;
            RenderService.Redraw();
            Storage.SaveToSessionStorage();
        }
        
    }
}