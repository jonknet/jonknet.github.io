using System;
using System.Collections.Generic;
using Microsoft.JSInterop;
using TreeBuilder.ComponentsRedux;
using TreeBuilder.Services;

namespace TreeBuilder.Classes {
    public class EventState {
        public static BaseClass Payload;
        public static BaseClass Selection;

        public static InterfaceSlot RightTop;
        public static InterfaceSlot RightBottom;

        public static int LastDomId = -1;

        public static bool DraggingEvent = false;

        public static Dictionary<Guid, IntegrationNode> RuntimeIntegrations = new();
        public static Dictionary<Guid, Group> RuntimeGroups = new();
        public static Dictionary<Guid, Interface> RuntimeInterfaces = new();

        public EventState(StorageService storage, IJSRuntime js) {
            Storage = storage;
            JS = js;

            var dotNetRef = DotNetObjectReference.Create(this);
            JS.InvokeVoidAsync("UpdateTitleHelper", dotNetRef);
        }

        private StorageService Storage { get; }
        private IJSRuntime JS { get; }

        /// <summary>
        ///     Extracts Group and Interfaces and inserts them into their respective Dictionaries
        /// </summary>
        /// <param name="gfd">GroupItems List from the GroupField</param>
        public void PopulateDictionaryGroupField(List<BaseClass> gfd) {
            foreach (var item in gfd) {
                try {
                    if (item is Group)
                        RuntimeGroups[item.Guid] = item as Group;
                    else if (item is Interface) RuntimeInterfaces[item.Guid] = item as Interface;
                }
                catch (ArgumentException) {
                    continue;
                }

                if (item.GroupItems != null && item.GroupItems.Count > 0)
                    PopulateDictionaryGroupField(item.GroupItems);
            }
        }

        /// <summary>
        ///     Extracts IntegrationNodes and Interfaces and puts them in their respective Dictionaries
        /// </summary>
        /// <param name="ifd">List of IntegrationNodes</param>
        public void PopulateDictionaryIntegrationsField(List<IntegrationNode> ifd) {
            foreach (var item in ifd) {
                RuntimeIntegrations[item.Guid] = item;

                try {
                    foreach (var iface in item.Interfaces)
                        if (iface != null)
                            RuntimeInterfaces[iface.Guid] = iface;
                }
                catch (ArgumentException) {
                    continue;
                }

                if (item.IntegrationNodes != null && item.IntegrationNodes.Count > 0)
                    PopulateDictionaryIntegrationsField(item.IntegrationNodes);
            }
        }

        /// <summary>
        ///     Invoked from Javascript to update the title of an element
        /// </summary>
        /// <param name="newTitle">The new title</param>
        /// <param name="objGuid">Guid of the object to update</param>
        [JSInvokable]
        public void UpdateTitle(string newTitle, string objGuid) {
            var guid = Guid.Parse(objGuid);

            if (RuntimeGroups.ContainsKey(guid)) {
                RuntimeGroups[guid].Title = newTitle;
                RuntimeGroups[guid].IsEditable = false;
            }
            else if (RuntimeIntegrations.ContainsKey(guid)){
                RuntimeIntegrations[guid].Title = newTitle;
                RuntimeIntegrations[guid].IsEditable = false;
            }
            else if (RuntimeInterfaces.ContainsKey(guid)) {
                RuntimeInterfaces[guid].Title = newTitle;
                RuntimeInterfaces[guid].IsEditable = false;
            }

            Storage.SaveToSessionStorage();
        }
    }
}