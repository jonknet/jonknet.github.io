using System;
using TreeBuilder.Classes;

namespace TreeBuilder.ComponentsRedux {
    public partial class GroupField : Group {
        protected override void OnInitialized() {
            Field = this;
            var groupField = Storage.LoadGroupField();
            if (groupField != null) {
                GroupItems.Clear();
                GroupItems = groupField.GroupItems;
                Title = groupField.Title;
            }
        }

        /*public override void HandleOnDrop()
        {
            Console.WriteLine($"This: {this}\r\nPayload: {EventState.Payload}\r\nField: {EventState.Payload.Field}\r\nParent: {EventState.Payload.Parent}");

            if((!EventState.Payload.Is<Interface>() && !EventState.Payload.Is<Group>()) || GroupItems.Contains(EventState.Payload))
            {
                return;
            }
            
            EventState.DeleteItemFromStorage(EventState.Payload);
            
            GroupItems.Add(EventState.Payload);

            EventState.Payload.Parent = this;
            EventState.Payload.Field = this;
            RenderService.Redraw();
            Storage.SaveToSessionStorage();
        }*/
    }
}