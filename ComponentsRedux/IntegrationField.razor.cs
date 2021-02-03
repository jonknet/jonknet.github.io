using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using TreeBuilder.Classes;
using TreeBuilder.Services;

namespace TreeBuilder.ComponentsRedux {
    public partial class IntegrationField : Group {

        [JsonIgnore] public Dictionary<Guid, IntegrationNode> NodeReferences { get; set; } = new Dictionary<Guid, IntegrationNode>();
        [Parameter] public List<IntegrationNode> IntegrationNodes { get; set; } = new List<IntegrationNode>();

        protected override void OnInitialized() {
            IntegrationField intField = Storage.LoadIntegrationField();
            if (intField != null) {
                IntegrationNodes.Clear();
                IntegrationNodes = intField.IntegrationNodes;
                Title = intField.Title;
            }
        }
        
        public override void HandleOnDrop()
        {
            
            // Dirty Hack to allow the ghost Integration Node to hide
            //(Payload as Interface).HandleOnDragEnd();
            // End Hack
            
            if (Is<IntegrationNode>(Payload) && (ContainsNode(Payload as IntegrationNode) || (Payload as IntegrationNode).HasNodesOnTop())) {
                return;
            }

            if (Is<IntegrationNode>(Payload)) {
                IntegrationNodes.Add(Payload as IntegrationNode);
                (Payload.Parent as IntegrationNode).RemoveNode(Payload.Parent as IntegrationNode, Payload as IntegrationNode);
                NodeReferences.Remove(Payload.Guid);
                Payload.Parent = this;
                Payload.Field = this;
            }

            RenderService.Redraw();
            
            Storage.SaveToSessionStorage();
        }
        
        // Helper methods
        public void RemoveNode<T>(T BaseNode, IntegrationNode TargetNode) where T : IntegrationField {
            foreach (IntegrationNode node in BaseNode.IntegrationNodes) {
                if (node.Equals(TargetNode)) {
                    BaseNode.IntegrationNodes.Remove(TargetNode);
                    return;
                }
                RemoveNode(node, TargetNode);
            }
        }

        public bool HasNodesOnTop() {
            return (IntegrationNodes.Count > 0) ? true : false;
        }

        public bool ContainsNode(IntegrationNode Node) {
            return IntegrationNodes.Contains(Node);
        }
        
        public override string ToString()
        {
            return "IntegrationField: \r\n" + base.ToString();
        }
    }
}