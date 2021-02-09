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

    public partial class IntegrationNode : BaseClass {

        private static Random Random { get; set; } = new Random();

        public IntegrationNode()
        {
            DomId = GetNextDomId();
        }

        public IntegrationNode(BaseClass Parent) : base(Parent, Parent as Group) {
            DomId = GetNextDomId();
        }

        [Parameter] public Interface[] Interfaces { get; set; } =  { null, null, null, null };

        [Parameter] public int DomId { get; set; }

        protected override void OnInitialized() {
            if (InstanceClass != null) {
                base.OnInitialized();
                DomId = (InstanceClass as IntegrationNode).DomId;
                Interfaces = (InstanceClass as IntegrationNode).Interfaces;
            }
        }

        private int GetNextDomId()
        {
            int newDomId = Random.Next();
            while (EventState.RuntimeIntegrations.Values.FirstOrDefault((e)=> e.DomId == newDomId) != null)
            {
                newDomId = Random.Next();
            }
            return newDomId;
        }
        
        public override void HandleOnDragEnter(BaseClass payload) {
            if (!Is<Interface>(EventState.Payload))
            {
                return;
            }
            
            JS.InvokeVoidAsync("ToggleSlots", EventState.LastDomId.ToString(), !(EventState.LastDomId != -1 && EventState.LastDomId != DomId));
           
            EventState.LastDomId = DomId;

            CssClass = "tb-dropborder";
            RenderService.Redraw();
        }

        public override void HandleOnDrop() {
            if (!Is<IntegrationNode>(EventState.Payload) || EventState.Payload == this ||
                (Is<IntegrationNode>(EventState.Payload) && this.ContainsNode(EventState.Payload as IntegrationNode)))
            {
                Console.WriteLine(this.ContainsNode(EventState.Payload as IntegrationNode) + " " + this.HasNodesOnTop());
                return;
            }

            base.HandleOnDrop();
        }
    }
}