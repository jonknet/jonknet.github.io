using System.Collections.Generic;
using System;
using TreeBuilder.Components;

namespace TreeBuilder.Services {
    public class ComponentTracker {

        private List<BaseItem> Items = new List<BaseItem>();

        public ComponentTracker(){
            
        }

        public void Add(BaseItem item) {
            Items.Add(item);
        }

        private int ContainsByName(string name) {
            return Items.FindIndex((e) => e.Name == name);
        }

        private int ContainsByGuid(Guid guid) {
            return Items.FindIndex((e) => e.Uid == guid);
        }

        private int ContainsByRef(BaseItem item) {
            return Items.FindIndex((e) => e == item);
        }
        
        public BaseItem GetByName(string name) {
            var index = ContainsByName(name);
            if (index == -1)
                return null;
            else {
                return Items[index];
            }
        }

        public void RemoveByRef(BaseItem item) {
            Items.Remove(item);
        }

        public void RemoveByGuid(Guid guid) {
            Items.RemoveAll((e) => e.Uid == guid);
        }

        public void RemoveByName(string name) {
            Items.RemoveAll((e) => e.Name == name);
        }
    }
}