using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
            if (Parent == null)
            {
                Parent = Field;
            }
            Field = Storage.IntegrationField;
            DomId = CurrentDomId;
            CurrentDomId++;
        }
        


        public override void HandleOnDragEnter(BaseClass payload) {
            if (!Is<Interface>(EventState.Payload))
            {
                return;
            }

            if (EventState.LastDomId != -1 && EventState.LastDomId != DomId)
            {
                ((IJSInProcessRuntime)JS).InvokeVoid("ToggleSlots", EventState.LastDomId.ToString(), false);
            }
            ((IJSInProcessRuntime)JS).InvokeVoid("ToggleSlots", DomId.ToString(), true);
            EventState.LastDomId = DomId;

            CssClass = "tb-dropborder";
            RenderService.Redraw();
        }

        public override void HandleOnDrop() {
            if (!Is<IntegrationNode>(EventState.Payload) || EventState.Payload == this ||
                (Is<IntegrationNode>(EventState.Payload) && ContainsNode(EventState.Payload as IntegrationNode)))
            {
                Console.WriteLine(ContainsNode(EventState.Payload as IntegrationNode) + " " + HasNodesOnTop());
                return;
            }

            BaseClass b = EventState.FindItem(this.Guid);
            
            EventState.DeleteItem(EventState.Payload.Guid.ToString());

            EventState.Payload.Parent = this;
            
            b.GroupItems.Add(EventState.Payload as IntegrationNode);
            
            RenderService.Redraw();

            Storage.SaveToSessionStorage();
        }
    }
}