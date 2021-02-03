using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using TreeBuilder.Classes;
using TreeBuilder.Services;

namespace TreeBuilder.ComponentsRedux {
    public partial class IntegrationField : Group {

        public Dictionary<Guid, IntegrationNode> NodeReferences { get; set; } = new Dictionary<Guid, IntegrationNode>();
        [Parameter] public List<IntegrationNode> IntegrationNodes { get; set; } = new List<IntegrationNode>();

        protected override void OnInitialized() {
            IntegrationField intField = Storage.LoadIntegrationField();
            if (intField != null) {
                IntegrationNodes = intField.IntegrationNodes;
                Title = intField.Title;
            }
        }
        
        public override void HandleOnDrop()
        {
            
            // Dirty Hack to allow the ghost Integration Node to hide
            //(Payload as Interface).HandleOnDragEnd();
            // End Hack
            
            if (Payload == this) {
                return;
            }

            if (Payload.GetType() == typeof(IntegrationNode)) {
                GroupItems.Add(Payload as IntegrationNode);
                Payload.Parent.GroupItems.Remove(Payload);
                Payload.Parent = this;
                Payload.Field = this;
            }
            
            Storage.SaveToSessionStorage();
        }
        
        public override string ToString()
        {
            return "IntegrationField: \r\n" + base.ToString();
        }

    }
}