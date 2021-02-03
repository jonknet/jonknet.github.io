using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using TreeBuilder.Classes;
using TreeBuilder.Components;

namespace TreeBuilder.ComponentsRedux {
    public partial class IntegrationNode : IntegrationField {

        [Parameter] public List<Interface> Interfaces { get; set; } = new List<Interface> {null, null, null, null};

        public IntegrationNode() : base() {
            
        }

        protected override void OnInitialized() {
            
        }

        public override void HandleOnDrop()
        {
            if(Payload.GetType() != typeof(IntegrationNode) || Payload == this){
                return;
            }

            if (Payload.Parent.GetType() == typeof(IntegrationField)) {
                (Payload.Field as IntegrationField).IntegrationNodes.Remove(Payload as IntegrationNode);
            } else if (Payload.Parent.GetType() == typeof(IntegrationNode)) {
                (Payload.Parent as IntegrationNode).IntegrationNodes.Remove(Payload as IntegrationNode);
            }
            
            IntegrationNodes.Add(Payload as IntegrationNode);
            Payload.Parent = this;
            
            RenderService.Redraw();
            
            Storage.SaveToSessionStorage();
        }
        
    }
}