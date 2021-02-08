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

        public GroupField GroupFieldBackup;
        public IntegrationField IntFieldBackup;

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
            
            if(Storage.GroupField != null && Storage.IntegrationField != null)
                PopulateDictionary(Storage.GroupField.GroupItems.Concat(Storage.IntegrationField.GroupItems).ToList());
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
        public void ToggleEditable(BaseClass obj) {
            if (obj.IsEditable) {
                JS.InvokeVoidAsync("HandleOnBlur", this);
            }
            else {
                obj.IsEditable = true;
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
            BaseClass output = null;
            output = Search(guid, Storage.GroupField.GroupItems);
            if (output == null)
            {
                output = Search(guid, Storage.IntegrationField.GroupItems);
            }
            return output;
        }

        private BaseClass Search(Guid guid, List<BaseClass> list)
        {
            BaseClass b = null;
            foreach (var item in list)
            {
                if (item.Guid == guid)
                {
                    return item;
                }

                if (item is IntegrationNode)
                {
                    foreach (var i in (item as IntegrationNode).Interfaces)
                    {
                        if (i != null && i.Guid == guid)
                        {
                            return i;
                        }
                    }
                }
                
                if(item.GroupItems.Count > 0)
                    b = Search(guid, item.GroupItems);
                if (b != null)
                    return b;
            }

            return b;
        }



        [JSInvokable]
        public void FinishTour()
        {
            DraggingEvent = false;

            Storage.GroupField = Storage.LoadGroupField();
            Storage.IntegrationField = Storage.LoadIntegrationField();

            RenderService.Redraw();
        }

        /// <summary>
        ///     Invoked from Javascript to update the title of an element
        /// </summary>
        /// <param name="newTitle">The new title</param>
        /// <param name="objGuid">Guid of the object to update</param>
        [JSInvokable]
        public void UpdateTitle(string newTitle, string objGuid, BaseClass obj)
        {
            var guid = Guid.Parse(objGuid);

            BaseClass b = FindItem(guid);

            b.SetTitle(newTitle);
            b.IsEditable = false;

            if (obj != null)
            {
                obj.SetTitle(newTitle);
                obj.IsEditable = false;
            }

            Storage.SaveToSessionStorage();
            RenderService.Redraw();
        }

        public void DeleteItem(string objGuid)
        {
            var guid = Guid.Parse(objGuid);

            BaseClass b = FindItem(guid);

            if (b.Is<Group>() || b.Is<IntegrationNode>())
            {
                b.Parent.GroupItems.Remove(b);

                if (b.Is<Group>())
                {
                    RuntimeGroups.Remove(b.Guid);
                } else if (b.Is<IntegrationNode>())
                {
                    RuntimeIntegrations.Remove(b.Guid);
                }
            }
            else if (b.Is<Interface>())
            {
                if (b.Parent is IntegrationNode)
                {
                    Interface[] iarray = (b.Parent as IntegrationNode).Interfaces;
                    int idx = iarray.IndexOf(b);
                    if (iarray.IndexOf(b) != -1)
                        iarray[iarray.IndexOf(b)] = null;
                }
                else
                {
                    b.Parent.GroupItems.Remove(b);
                }

                RuntimeInterfaces.Remove(b.Guid);
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