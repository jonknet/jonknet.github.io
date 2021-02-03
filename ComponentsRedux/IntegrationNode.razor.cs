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
            if(Payload.GetType() != typeof(IntegrationNode) || Payload == this || Payload.Parent == this){
                return;
            }

            if (Payload.Parent.GetType() == typeof(IntegrationNode) || Payload.Parent.GetType() == typeof(IntegrationField)) {
                (Payload.Parent as IntegrationNode).GroupItems.Remove(Payload as IntegrationNode);
                GroupItems.Add(Payload as IntegrationNode);
                Payload.Parent = this;
            }
            
            Storage.SaveToSessionStorage();
        }
        
    }
}