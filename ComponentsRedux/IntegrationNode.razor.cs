using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TreeBuilder.Classes;

namespace TreeBuilder.ComponentsRedux {
    public enum InterfaceSlotPosition {
        Left = 0,
        Right = 2,
        RightTop = 1,
        RightBottom = 3
    }

    public partial class IntegrationNode : IntegrationField {
        public static int CurrentDomId = 32847;

        [Parameter] public List<Interface> Interfaces { get; set; } = new List<Interface>(4) {null, null, null, null};

        public int DomId { get; set; }
        
        public IntegrationNode() : base() {
            DomId = CurrentDomId;
            CurrentDomId++;
        }

        public IntegrationNode(BaseClass Parent, Group Field) : base(Parent,Field) {
            this.Parent = Parent;
            this.Field = Field;
            DomId = CurrentDomId;
            CurrentDomId++;
        }

        protected override void OnInitialized() {
            Parent = Storage.IntegrationField;
            Field = Storage.IntegrationField;
            DomId = CurrentDomId;
            CurrentDomId++;
        }

        public override void HandleOnDragEnter() {
            Console.WriteLine("HandleOnDragEnter");
            if (EventState.LastDomId != -1) JS.InvokeVoidAsync("ToggleSlots", EventState.LastDomId.ToString(), false);
            JS.InvokeVoidAsync("ToggleSlots", DomId.ToString(), true);
            EventState.LastDomId = DomId;
            base.HandleOnDragEnter();
        }

        public override void HandleOnDrop() {
            if (!Is<IntegrationNode>(EventState.Payload) || EventState.Payload == this ||
                Is<IntegrationNode>(EventState.Payload) && ContainsNode(EventState.Payload as IntegrationNode) ||
                Is<IntegrationNode>(EventState.Payload) && HasNodesOnTop())
                return;

            if (Is<IntegrationField>(EventState.Payload.Parent))
                (EventState.Payload.Parent as IntegrationField).RemoveNode(
                    EventState.Payload.Parent as IntegrationField,
                    EventState.Payload as IntegrationNode);
            else if (Is<IntegrationNode>(EventState.Payload.Parent))
                (EventState.Payload.Parent as IntegrationNode).RemoveNode(EventState.Payload.Parent as IntegrationNode,
                    EventState.Payload as IntegrationNode);

            NodeReferences.Remove(EventState.Payload.Guid);
            IntegrationNodes.Add(EventState.Payload as IntegrationNode);
            EventState.Payload.Parent = this;

            RenderService.Redraw();

            Storage.SaveToSessionStorage();
        }
    }
}