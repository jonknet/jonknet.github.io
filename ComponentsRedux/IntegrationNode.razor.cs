using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
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

        public IntegrationNode() {
            DomId = CurrentDomId;
            CurrentDomId++;
        }

        public IntegrationNode(BaseClass Parent, Group Field) : base(Parent, Field) {
            this.Parent = Parent;
            this.Field = Field;
            DomId = CurrentDomId;
            CurrentDomId++;
        }

        [Parameter] public Interface[] Interfaces { get; set; } =  { null, null, null, null };

        [Parameter] public int DomId { get; set; }

        protected override void OnInitialized() {
            Parent = Storage.IntegrationField;
            Field = Storage.IntegrationField;
            DomId = CurrentDomId;
            CurrentDomId++;
        }

        public override void HandleOnDragEnter() {
            
            if (!Is<Interface>(EventState.Payload)) {
                return;
            }

            if (EventState.LastDomId != DomId) {
                ((IJSInProcessRuntime) JS).InvokeVoid("ToggleSlots", DomId.ToString(), true);
                ((IJSInProcessRuntime) JS).InvokeVoid("ToggleSlots", EventState.LastDomId.ToString(), false);
                EventState.LastDomId = DomId;
            }

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