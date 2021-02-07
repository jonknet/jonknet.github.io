using System;
using TreeBuilder.Classes;

namespace TreeBuilder.ComponentsRedux {
    public partial class Group : BaseClass {
        public Group() { }
        public Group(BaseClass Parent, Group Field) : base(Parent, Field) { }
        

        public override void HandleOnDrop()
        {
            Console.WriteLine($"This: {this}\r\nPayload: {EventState.Payload}\r\nField: {EventState.Payload.Field}\r\nParent: {EventState.Payload.Parent}");

            if ((!EventState.Payload.Is<Interface>() && !EventState.Payload.Is<Group>()) ||
                GroupItems.Contains(EventState.Payload) ||
                EventState.Payload == this)
            {
                return;
            }
            
            EventState.DeleteItem(EventState.Payload.Guid.ToString());

            GroupItems.Add(EventState.Payload);

            EventState.Payload.Parent = this;

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