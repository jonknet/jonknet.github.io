using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.JSInterop;
using Telerik.DataSource.Extensions;
using TreeBuilder.ComponentsRedux;
using TreeBuilder.Services;

namespace TreeBuilder.Classes {
    public class EventState {
        public static BaseClass Payload;
        public static BaseClass Selection;

        public static InterfaceSlot RightTop;
        public static InterfaceSlot RightBottom;

        public static int LastDomId = -1;
        public static BaseClass ItemActive = null;

        public static bool DraggingEvent = false;

        public static Dictionary<Guid, IntegrationNode> RuntimeIntegrations = new();
        public static Dictionary<Guid, Group> RuntimeGroups = new();
        public static Dictionary<Guid, Interface> RuntimeInterfaces = new();

        public EventState(StorageService storage, IJSRuntime js, RenderService render) {
            Storage = storage;
            JS = js;
            RenderService = render;

            var dotNetRef = DotNetObjectReference.Create(this);
            JS.InvokeVoidAsync("UpdateTitleHelper", dotNetRef);
        }

        private StorageService Storage { get; }
        private IJSRuntime JS { get; }
        private RenderService RenderService { get; }

        /// <summary>
        ///     Extracts All Items and inserts them into their respective Dictionaries
        /// </summary>
        /// <param name="list">List of items</param>
        public void PopulateDictionary(List<BaseClass> list) {
            foreach (var item in list) {
                try {
                    if (item is IntegrationNode) {
                        RuntimeIntegrations[item.Guid] = item as IntegrationNode;
                        foreach (var i in (item as IntegrationNode).Interfaces) {
                            if(i != null)
                                RuntimeInterfaces[i.Guid] = i;
                        }
                    } else if (item is Group)
                        RuntimeGroups[item.Guid] = item as Group;
                    else if (item is Interface) 
                        RuntimeInterfaces[item.Guid] = item as Interface;
                }
                catch (ArgumentException) {
                    continue;
                }

                PopulateDictionary(item.GroupItems);
            }
        }
        
        
        [JSInvokable]
        public void GetCommands() {
            Console.WriteLine("OutputStorageList");
            Console.WriteLine("OutputDictionaries");
        }

        [JSInvokable]
        public void OutputStorageList() {
            List<BaseClass> list = Storage.GroupField.GroupItems.Concat(Storage.IntegrationField.GroupItems).ToList();
            foreach (var i in list) {
                Console.WriteLine($"Guid:{i.Guid},Title:{i.Title},Type:{i.GetType()}");
            }
        }

        [JSInvokable]
        public void OutputDictionaries() {
            List<BaseClass> list = RuntimeGroups.Values.Concat(RuntimeIntegrations.Values)
                .Concat<BaseClass>(RuntimeInterfaces.Values).ToList();
            foreach (var i in list) {
                Console.WriteLine($"Guid:{i.Guid},Title:{i.Title},Type:{i.GetType()}");
            }
        }

        public BaseClass FindItem(Guid guid, List<BaseClass> list = null) {
            Console.WriteLine($"FindItem Guid:{guid}");
            
            if (list == null) {
                list = RuntimeGroups.Values.Concat(RuntimeIntegrations.Values)
                    .Concat<BaseClass>(RuntimeInterfaces.Values).ToList();
            }

            foreach (var i in list) {
                if (i.Guid == guid) {
                    return i;
                }
            }

            return null;
        }

        /// <summary>
        ///     Invoked from Javascript to update the title of an element
        /// </summary>
        /// <param name="newTitle">The new title</param>
        /// <param name="objGuid">Guid of the object to update</param>
        [JSInvokable]
        public void UpdateTitle(string newTitle, string objGuid) {
            Console.WriteLine($"UpdateTitle {newTitle} {objGuid}");
            
            var guid = Guid.Parse(objGuid);

            BaseClass b = FindItem(guid);
            
            b.SetTitle(newTitle);
            b.IsEditable = false;

            Storage.SaveToSessionStorage();
            RenderService.Redraw();
        }

        public void DeleteItem(string objGuid) {
            var guid = Guid.Parse(objGuid);

            BaseClass b = FindItem(guid);
            Console.WriteLine("Found " + b);

            if (b.Is<Group>() || b.Is<IntegrationNode>()) {
                b.Parent.GroupItems.Remove(b);
            } else if (b.Is<Interface>()) {
                if (b.Parent is InterfaceSlot) {
                    (b.Parent.Parent as IntegrationNode).Interfaces[(b.Parent as InterfaceSlot).Position] = null;
                }
                else {
                    b.Parent.GroupItems.Remove(b);
                }
                
            }

            Storage.SaveToSessionStorage();
            RenderService.Redraw();
        }
        
    }
}