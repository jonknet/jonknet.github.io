using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using TreeBuilder.Classes;

namespace TreeBuilder.ComponentsRedux {

    public enum InterfaceSlotPosition {
        Left = 0,
        Right = 2,
        RightTop = 1,
        RightBottom = 3
    }
    public partial class IntegrationNode : IntegrationField {
        [Parameter] public Interface[] Interfaces { get; set; } = new Interface[4] {null, null, null, null};
        
        
        
        public IntegrationNode() { }

        public IntegrationNode(BaseClass Parent, Group Field) {
            this.Parent = Parent;
            this.Field = Field;
        }

        protected override void OnInitialized() {
            Parent = Storage.IntegrationField;
            Field = Storage.IntegrationField;
        }

        public override void HandleOnDrop() {
            if (!Is<IntegrationNode>(EventState.Payload) || EventState.Payload == this ||
                Is<IntegrationNode>(EventState.Payload) && ContainsNode(EventState.Payload as IntegrationNode) ||
                Is<IntegrationNode>(EventState.Payload) && HasNodesOnTop())
                return;

            if (Is<IntegrationField>(EventState.Payload.Parent))
                (EventState.Payload.Parent as IntegrationField).RemoveNode<IntegrationField>(EventState.Payload.Parent as IntegrationField,
                    EventState.Payload as IntegrationNode);
            else if (Is<IntegrationNode>(EventState.Payload.Parent))
                (EventState.Payload.Parent as IntegrationNode).RemoveNode<IntegrationNode>(EventState.Payload.Parent as IntegrationNode,
                    EventState.Payload as IntegrationNode);

            NodeReferences.Remove(EventState.Payload.Guid);
            IntegrationNodes.Add(EventState.Payload as IntegrationNode);
            EventState.Payload.Parent = this;

            RenderService.Redraw();

            Storage.SaveToSessionStorage();
        }
    }
}