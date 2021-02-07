using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Telerik.DataSource.Extensions;
using TreeBuilder.ComponentsRedux;
using TreeBuilder.Services;

namespace TreeBuilder.Classes
{
    public class EventState
    {
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

        public EventState(StorageService storage, IJSRuntime js, RenderService render)
        {
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
        public void PopulateDictionary(List<BaseClass> list)
        {
            foreach (var item in list)
            {
                try
                {
                    if (item is IntegrationNode)
                    {
                        RuntimeIntegrations[item.Guid] = item as IntegrationNode;
                        foreach (var i in (item as IntegrationNode).Interfaces)
                        {
                            if (i != null)
                                RuntimeInterfaces[i.Guid] = i;
                        }
                    }
                    else if (item is Group)
                        RuntimeGroups[item.Guid] = item as Group;
                    else if (item is Interface)
                        RuntimeInterfaces[item.Guid] = item as Interface;
                }
                catch (ArgumentException)
                {
                    continue;
                }

                PopulateDictionary(item.GroupItems);
            }
        }


        [JSInvokable]
        public void OutputReferences()
        {
            foreach (var i in BaseClass.Instances.Values)
            {
                Console.WriteLine($"Guid:{i.Guid},Title:{i.Title},Type:{i.GetType()}");
            }
        }

        [JSInvokable]
        public void OutputAll(List<BaseClass> list = null)
        {
            if(list == null)
                list = Storage.GroupField.GroupItems.Concat(Storage.IntegrationField.GroupItems).ToList();
            
            foreach (var i in list)
            {
                Console.WriteLine($"Guid:{i.Guid},Title:{i.Title},Type:{i.GetType()}");
                if (i is IntegrationNode)
                {
                    foreach (var j in (i as IntegrationNode).Interfaces)
                    {
                        if (j != null)
                            Console.WriteLine($"Guid:{j.Guid},Title:{j.Title},Type:Interface");
                    }
                }

                OutputAll(i.GroupItems);
            }
        }

        [JSInvokable]
        public void OutputDictionaries()
        {
            List<BaseClass> list = RuntimeGroups.Values.Concat(RuntimeIntegrations.Values)
                .Concat<BaseClass>(RuntimeInterfaces.Values).ToList();
            foreach (var i in list)
            {
                Console.WriteLine($"Guid:{i.Guid},Title:{i.Title},Type:{i.GetType()}");
            }
        }

        public BaseClass FindItem(Guid guid)
        {
            Console.WriteLine($"FindItem Guid:{guid}");

            BaseClass output = null;
            output = Search(guid, Storage.GroupField.GroupItems);
            if (output != null)
            {
                Console.WriteLine($"Guid:{output.Guid},Title:{output.Title},Type:{output.GetType()}");
                return output;
            }

            output = Search(guid, Storage.IntegrationField.GroupItems);
            if (output != null)
                Console.WriteLine($"Guid:{output.Guid},Title:{output.Title},Type:{output.GetType()}");
            return output;
        }

        private BaseClass Search(Guid guid, List<BaseClass> list)
        {
            BaseClass b = null;
            foreach (var item in list)
            {
                if (item.Guid == guid)
                {
                    Console.WriteLine("FindItem searched for " + guid + " and found " + item.Guid);

                    b = item;
                    break;
                }

                if (item is IntegrationNode)
                {
                    foreach (var i in (item as IntegrationNode).Interfaces)
                    {
                        if (i != null && i.Guid == guid)
                        {
                            Console.WriteLine("FindItem searched for " + guid + " and found " + i.Guid);
                            b = i;
                            break;
                        }
                    }
                }

                if (b != null)
                    return b;
                b = Search(guid, item.GroupItems);
            }

            return b;
        }

        /// <summary>
        ///     Invoked from Javascript to update the title of an element
        /// </summary>
        /// <param name="newTitle">The new title</param>
        /// <param name="objGuid">Guid of the object to update</param>
        [JSInvokable]
        public void UpdateTitle(string newTitle, string objGuid, BaseClass obj)
        {
            Console.WriteLine($"UpdateTitle {newTitle} {objGuid}");

            var guid = Guid.Parse(objGuid);

            BaseClass b = FindItem(guid);

            b.SetTitle(newTitle);
            b.IsEditable = false;

            if (obj != null)
            {
                obj.SetTitle(newTitle);
                obj.IsEditable = false;
            }

            Console.WriteLine(b.Title);

            Storage.SaveToSessionStorage();
            RenderService.Redraw();
        }

        public void DeleteItem(string objGuid)
        {
            var guid = Guid.Parse(objGuid);

            BaseClass b = FindItem(guid);
            Console.WriteLine("Found " + b);

            if (b.Is<Group>() || b.Is<IntegrationNode>())
            {
                b.Parent.GroupItems.Remove(b);
            }
            else if (b.Is<Interface>())
            {
                Console.WriteLine(b.Parent.GetType());
                if (b.Parent is IntegrationNode)
                {
                    Interface[] iarray = (b.Parent as IntegrationNode).Interfaces;
                    int idx = iarray.IndexOf(b);
                    if (iarray.IndexOf(b) != -1)
                        iarray[iarray.IndexOf(b)] = null;
                    Console.WriteLine("Removed interface from " + idx);
                }
                else
                {
                    b.Parent.GroupItems.Remove(b);
                }
            }

            DeleteItemFromStorage(b);

            Storage.SaveToSessionStorage();
            RenderService.Redraw();
        }

        private void DeleteItemFromStorage(BaseClass obj)
        {
            DeleteItemFromStorageInt(obj, Storage.GroupField.GroupItems);
            DeleteItemFromStorageInt(obj, Storage.IntegrationField.GroupItems);
        }

        private void DeleteItemFromStorageInt(BaseClass obj, List<BaseClass> list)
        {
            if (list.Contains(obj))
            {
                list.Remove(obj);
                return;
            }

            foreach (var i in list)
            {
                if (i.Is<IntegrationNode>())
                {
                    Interface[] iarray = (i as IntegrationNode).Interfaces;
                    if (iarray.IndexOf(obj) != -1)
                        iarray.SetValue(null, iarray.IndexOf(obj));
                }

                DeleteItemFromStorageInt(obj, i.GroupItems);
            }
        }
    }
}