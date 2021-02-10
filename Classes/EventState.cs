using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Newtonsoft.Json;
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
        private void PopulateDictionary(List<BaseClass> list)
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
        
        /// <summary>
        /// Toggles whether the title is editable or not
        /// </summary>
        /// <param name="obj"></param>
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
            List<BaseClass> list = RuntimeGroups.Values.Concat<BaseClass>(RuntimeIntegrations.Values)
                .Concat<BaseClass>(RuntimeInterfaces.Values).ToList();
            foreach (var i in list)
            {
                Console.WriteLine($"Guid:{i.Guid},Title:{i.Title},Type:{i.GetType()}");
            }
        }

        /// <summary>
        /// Searches for an item in the Fields by Guid
        /// </summary>
        /// <param name="guid">Guid of object</param>
        /// <returns>Found BaseClass object</returns>
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

        private BaseClass FastFind(BaseClass obj, Guid guid = new Guid()) {
            if (guid != Guid.Empty) {
                BaseClass b = null;
                if (RuntimeGroups.ContainsKey(guid)) {
                    b = RuntimeGroups[guid];
                }

                if (b != null)
                    return b;

                if (RuntimeIntegrations.ContainsKey(guid)) {
                    b = RuntimeIntegrations[guid];
                }

                if (b != null)
                    return b;

                if (RuntimeInterfaces.ContainsKey(guid)) {
                    b = RuntimeInterfaces[guid];
                }

                return b;
            }
            
            if (obj.Is<Group>()) {
                return RuntimeGroups[obj.Guid];
            } else if (obj.Is<Interface>()) {
                return RuntimeInterfaces[obj.Guid];
            } else if (obj.Is<IntegrationNode>()) {
                return RuntimeIntegrations[obj.Guid];
            }

            return null;
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


        /// <summary>
        /// Called by Javascript when the help tour is finished
        /// </summary>
        [JSInvokable]
        public void FinishTour()
        {
            DraggingEvent = false;

            Storage.GroupField = GroupFieldBackup;
            Storage.IntegrationField = IntFieldBackup;

            RenderService.Redraw(RenderService.Element.GroupField | RenderService.Element.IntegrationField);
        }

        /// <summary>
        /// Called from JS when page is exited
        /// </summary>
        [JSInvokable]
        public void SaveState() {
            Storage.SaveToSessionStorage();
        }

        /// <summary>
        /// Invoked from Javascript to update the title of an element
        /// </summary>
        /// <param name="newTitle">The new title</param>
        /// <param name="objGuid">Guid of the object to update</param>
        [JSInvokable]
        public void UpdateTitle(string newTitle, string objGuid, BaseClass obj = null) {
            BaseClass b = null;
            
            var guid = Guid.Parse(objGuid);
            b = FastFind(null, guid);
            
            if (obj != null) {
                obj.Title = newTitle;
                obj.IsEditable = false;
            }

            b.Title = newTitle;
            b.IsEditable = false;
            
            if(obj != null)
                RenderService.RedrawObject(obj);
            else {
                if (b.Field.Is<GroupField>()) {
                    RenderService.Redraw(RenderService.Element.GroupField);
                }
                else {
                    RenderService.Redraw(RenderService.Element.IntegrationField);
                }
            }
            //Storage.SaveToSessionStorage();
        }

        public void DeleteItem(BaseClass obj,bool permanentDelete = false)
        {

            BaseClass b = obj;

            if (b.Is<Group>() || b.Is<IntegrationNode>())
            {
                b.Parent.GroupItems.Remove(b);

                if (b.Is<Group>() && permanentDelete)
                {
                    RuntimeGroups.Remove(b.Guid);
                } else if (b.Is<IntegrationNode>() && permanentDelete)
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
                if(permanentDelete)
                    RuntimeInterfaces.Remove(b.Guid);
            }
            
            DeleteItemFromStorage(b);

            

            //Storage.SaveToSessionStorage();
            if (b.Field.Is<GroupField>()) {
                RenderService.Redraw(RenderService.Element.GroupField);
            }
            else {
                RenderService.Redraw(RenderService.Element.IntegrationField);
            }
        }

        private void DeleteItemFromStorage(BaseClass obj)
        {
            if(obj.Field != null && obj.Field.Is<GroupField>())
                DeleteItemFromStorageInt(obj, Storage.GroupField.GroupItems);
            else if(obj.Field != null && obj.Field.Is<IntegrationField>())
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
                    if (iarray.IndexOf(obj) != -1) {
                        iarray.SetValue(null, iarray.IndexOf(obj));
                        return;
                    }
                }

                DeleteItemFromStorageInt(obj, i.GroupItems);
            }
        }
    }
}