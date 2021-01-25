using System.Collections.Generic;
using TreeBuilder.Components;

namespace TreeBuilder.Services {
    public class IntNodeMplex {
        public List<IntegrationNode> inodes = new List<IntegrationNode>();

        public void Add(IntegrationNode inode) {
            inodes.Add(inode);
        }

        public void Remove(IntegrationNode inode) {
            var i = 0;
            foreach (var node in inodes) {
                if (inode.Uid == node.Uid) {
                    inodes.RemoveAt(i);
                    return;
                }
                i++;
            }
        }

        public void ToggleExtraSlots() {
            foreach (var node in inodes) {
                node.ToggleExtraSlots();
            }
        }
    }
}