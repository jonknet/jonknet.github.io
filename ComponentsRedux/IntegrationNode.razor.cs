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
            if(!Is<IntegrationNode>(Payload) || Payload == this || 
               (Is<IntegrationNode>(Payload) && ContainsNode(Payload as IntegrationNode)) ||
               (Is<IntegrationNode>(Payload) && HasNodesOnTop())){
                return;
            }

            if (Is<IntegrationField>(Payload.Parent)) {
                (Payload.Parent as IntegrationField).RemoveNode<IntegrationField>(Payload.Parent as IntegrationField, Payload as IntegrationNode);
            } else if (Is<IntegrationNode>(Payload.Parent)) {
                (Payload.Parent as IntegrationNode).RemoveNode<IntegrationNode>(Payload.Parent as IntegrationNode, Payload as IntegrationNode);
            }
            
            IntegrationNodes.Add(Payload as IntegrationNode);
            Payload.Parent = this;
            
            RenderService.Redraw();
            
            Storage.SaveToSessionStorage();
        }
        
    }
}