using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using TreeBuilder.Classes;
using TreeBuilder.Services;

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

        /* Not needed ? */
        /*public override void HandleOnDrop() {
            if (Is<IntegrationNode>(Payload) || (this.Field != null && Payload.Field != this.Field)) {
                return;
            }

            Console.WriteLine("Ondrop");
            Payload.Parent.GroupItems.Remove(Payload);
            
            GroupItems.Add(Payload);
            Payload.Parent = this;
            Payload.Field = this;

            RenderService.Redraw();
            
            Storage.SaveToSessionStorage();
        }*/
    }
}