using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using TreeBuilder.Classes;

namespace TreeBuilder.ComponentsRedux {
    public partial class IntegrationField : Group {
        public IntegrationField() { }
        public IntegrationField(BaseClass Parent, Group Field) : base(Parent, Field) { }
        
        [JsonIgnore] public Dictionary<Guid, IntegrationNode> NodeReferences { get; set; } = new();

        protected override void OnInitialized() {
            Field = this;
            var intField = Storage.LoadIntegrationField();
            if (intField != null) {
                GroupItems.Clear();
                GroupItems = intField.GroupItems;
                Title = intField.Title;
            }
        }

        public override void HandleOnDrop() {
            if (Is<IntegrationNode>(EventState.Payload) && (ContainsNode(EventState.Payload as IntegrationNode)))
                return;

            if (Is<IntegrationNode>(EventState.Payload))
            {
                EventState.DeleteItem(EventState.Payload.Guid.ToString());
                GroupItems.Add(EventState.Payload as IntegrationNode);
                //NodeReferences.Remove(EventState.Payload.Guid);
                EventState.Payload.Parent = this;
                EventState.Payload.Field = this;
            }

            RenderService.Redraw();

            Storage.SaveToSessionStorage();
        }

        // Helper methods
        public void RemoveNode<T>(T BaseNode, IntegrationNode TargetNode) where T : IntegrationField {
            foreach (var node in BaseNode.GroupItems) {
                if (node.Equals(TargetNode)) {
                    BaseNode.GroupItems.Remove(TargetNode);
                    return;
                }

                RemoveNode(node as T, TargetNode);
            }
        }

        /// <summary>
        ///     Returns true if an IntegrationNode contains other nodes
        /// </summary>
        /// <returns>true if this IntegrationNode contains other nodes</returns>
        public bool HasNodesOnTop() {
            return GroupItems.Count > 0 ? true : false;
        }

        /// <summary>
        ///     Returns boolean if this IntegrationField contains the node
        /// </summary>
        /// <param name="Node">Node to find</param>
        /// <returns>true if found, false if otherwise</returns>
        public bool ContainsNode(IntegrationNode Node) {
            return GroupItems.Contains(Node);
        }

        public override string ToString() {
            return "IntegrationField: \r\n" + base.ToString();
        }
    }
}