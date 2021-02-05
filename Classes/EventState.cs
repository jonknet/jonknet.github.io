using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TreeBuilder.ComponentsRedux;
using TreeBuilder.Services;

namespace TreeBuilder.Classes {
    public class EventState {

        private StorageService Storage { get; set; }
        private IJSRuntime JS { get; set; }
        
        public static BaseClass Payload;
        public static BaseClass Selection;

        public static InterfaceSlot RightTop;
        public static InterfaceSlot RightBottom;

        public static int LastDomId = -1;

        public static bool DraggingEvent = false;

        public static Dictionary<Guid, IntegrationNode> RuntimeIntegrations = new Dictionary<Guid, IntegrationNode>();
        public static Dictionary<Guid, Group> RuntimeGroups = new Dictionary<Guid, Group>();
        public static Dictionary<Guid, Interface> RuntimeInterfaces = new Dictionary<Guid, Interface>();

        public EventState(StorageService storage, IJSRuntime js) {
            Storage = storage;
            JS = js;

            var dotNetRef = DotNetObjectReference.Create(this);
            JS.InvokeVoidAsync("UpdateTitleHelper", dotNetRef);
            
            
            
        }

        public void PopulateDictionaryGroupField(List<BaseClass> gfd) {
            foreach (var item in gfd) {
                try {
                    if (item is Group) {
                        RuntimeGroups[item.Guid] = item as Group;
                    }
                    else if (item is Interface) {
                        RuntimeInterfaces[item.Guid] = item as Interface;
                    }
                }
                catch (ArgumentException e) {
                    continue;
                }

                if(item.GroupItems != null && item.GroupItems.Count > 0)
                    PopulateDictionaryGroupField(item.GroupItems);
            }
        }
        
        public void PopulateDictionaryIntegrationsField(List<IntegrationNode> ifd) {
            foreach (var item in ifd) {
                
                RuntimeIntegrations[item.Guid] = item as IntegrationNode;

                try {
                    foreach (var iface in item.Interfaces) {
                        if (iface != null) {
                            RuntimeInterfaces[iface.Guid] = iface;
                        }
                    }
                }
                catch (ArgumentException e) {
                    continue;
                }

                if(item.IntegrationNodes != null && item.IntegrationNodes.Count > 0)
                    PopulateDictionaryIntegrationsField(item.IntegrationNodes);
            }
        }
        
        [JSInvokable]
        public void UpdateTitle(string newTitle, string objGuid) {
            Guid guid = Guid.Parse(objGuid);
            
            if(RuntimeGroups.ContainsKey(guid))
                RuntimeGroups[guid].Title = newTitle;
            else if(RuntimeIntegrations.ContainsKey(guid))
                RuntimeIntegrations[guid].Title = newTitle;
            else if (RuntimeInterfaces.ContainsKey(guid))
                RuntimeInterfaces[guid].Title = newTitle;
            
            Storage.SaveToSessionStorage();
        }
        
    }
}