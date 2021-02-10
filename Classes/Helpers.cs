using TreeBuilder.ComponentsRedux;

namespace TreeBuilder.Classes {
    public static class Helpers {
        /// <summary>
        ///     Helper method to determine class type
        /// </summary>
        /// <param name="Base">Class you want to check</param>
        /// <typeparam name="T">Type to check for</typeparam>
        /// <returns></returns>
        public static bool Is<T>(this BaseClass Base) {
            return Base is T;
        }

        /// <summary>
        ///     Returns true if an IntegrationNode contains other nodes
        /// </summary>
        /// <returns>true if this IntegrationNode contains other nodes</returns>
        public static bool HasNodesOnTop(this BaseClass obj) {
            return obj.GroupItems.Count > 0 ? true : false;
        }

        /// <summary>
        ///     Returns boolean if this IntegrationField contains the node
        /// </summary>
        /// <param name="Node">Node to find</param>
        /// <returns>true if found, false if otherwise</returns>
        public static bool ContainsNode(this BaseClass obj, IntegrationNode Node) {
            return obj.GroupItems.Contains(Node);
        }
        
        
    }
}