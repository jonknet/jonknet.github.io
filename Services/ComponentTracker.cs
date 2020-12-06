using System.Collections.Generic;
using System;
using TreeBuilder.Components;

namespace TreeBuilder.Services {
    public class ComponentTracker {

        Dictionary<Guid,Item> componentdb;

        public ComponentTracker(){
            componentdb = new Dictionary<Guid,Item>();
        }

        public void Add(Guid guid, Item item){
            componentdb.Add(guid,item);
        }

        public void Remove(Guid guid){
            componentdb.Remove(guid);
        }

        public void Replace(Guid guid, Item item){
            componentdb.Remove(guid);
            Add(guid,item);
        }
    }
}