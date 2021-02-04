using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using TreeBuilder.Classes;
using TreeBuilder.Services;

namespace TreeBuilder.ComponentsRedux {
    public partial class GroupField : Group {

        protected override void OnInitialized() {
            GroupField groupField = Storage.LoadGroupField();
            if (groupField != null) {
                GroupItems.Clear();
                GroupItems = groupField.GroupItems;
                Title = groupField.Title;
            }
        }

        public override void HandleOnDrop() {
            if (Is<IntegrationNode>(Payload) || Payload.Field != this.Field) {
                return;
            }

            if (Is<Interface>(Payload)) {
                if (Is<IntegrationNode>(Payload.Parent)) {
                    Interface[] interfaces = (Payload.Parent as IntegrationNode).Interfaces;
                    for (int i = 0; i < interfaces.Length; i++) {
                        if (interfaces[i] != null && interfaces[i].Equals(Payload)) {
                            interfaces[i] = null;
                            break;
                        }
                    }
                }
            } else if (Is<Group>(Payload)) {
                Payload.Parent.GroupItems.Remove(Payload);
            }

            GroupItems.Add(Payload);
            Payload.Parent = this;
            Payload.Field = this;

            RenderService.Redraw();
            
            Storage.SaveToSessionStorage();
        }
    }
}