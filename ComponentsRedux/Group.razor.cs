using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using TreeBuilder.Classes;
using TreeBuilder.Components;

namespace TreeBuilder.ComponentsRedux {
    public partial class Group : BaseClass {

        public override void HandleOnDrop() {

            if (Payload.GetType() != typeof(Interface) && Payload.GetType() != typeof(Group))
            {
                return;
            }

            // Dirty ugly hack
            if (Payload.GetType() == typeof(Interface)) {
                //(Payload as Interface).HandleOnDragEnd();
            }
            // End hack

            GroupItems.Add(Payload);

            Payload.Parent.GroupItems.Remove(Payload);

            Payload.Parent = this;

            Payload.Field = Field;

            RenderService.Redraw();

            Storage.SaveToSessionStorage();

            CssClass = "";
            

        }

        public override string ToString()
        {
            string str = "Group: \r\n" + base.ToString() + "Items: \r\n";
            foreach(var item in GroupItems)
            {
                str += $"{item}\r\n";
            }
            return str;
        }
    }
}