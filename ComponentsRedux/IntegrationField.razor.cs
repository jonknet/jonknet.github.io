using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using TreeBuilder.Classes;

namespace TreeBuilder.ComponentsRedux {
    public partial class IntegrationField : Group {
        public IntegrationField() { }
        public IntegrationField(BaseClass Parent) : base(Parent) { }

        protected override void OnInitialized() {
            Field = Storage.IntegrationField;
            GroupItems = Storage.IntegrationField.GroupItems;
        }

        public override void HandleOnDrop() {
            if (Is<IntegrationNode>(EventState.Payload) && (this.ContainsNode(EventState.Payload as IntegrationNode)))
                return;

            if (Is<IntegrationNode>(EventState.Payload))
            {
                EventState.DeleteItem(EventState.Payload.Guid.ToString());
                GroupItems.Add(EventState.Payload as IntegrationNode);
                //NodeReferences.Remove(EventState.Payload.Guid);
                EventState.Payload.Parent = this;
                EventState.Payload.Field = this;
            }

            RenderService.Redraw();

            Storage.SaveToSessionStorage();
        }



        public override string ToString() {
            return "IntegrationField: \r\n" + base.ToString();
        }
    }
}