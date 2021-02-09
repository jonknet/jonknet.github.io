﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Telerik.Blazor.Components;
using TreeBuilder.ComponentsRedux;
using TreeBuilder.ComponentsRedux.TelerikUI;
using TreeBuilder.Services;

namespace TreeBuilder.Classes {
    /// <summary>
    ///     BaseClass of all types
    /// </summary>
    public class BaseClass : ComponentBase {

        public BaseClass()
        {
            
        }
        public BaseClass(BaseClass Parent, Group Field) {
            this.Parent = Parent;
            this.Field = Field;
        }
        
        [Inject] protected StorageService Storage { get; set; }
        [Inject] protected RenderService RenderService { get; set; }
        [Inject] protected IJSRuntime JS { get; set; }
        [Inject] protected EventState EventState { get; set; }
        
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = "Default";
        
        [Parameter] 
        [JsonProperty(IsReference = true)] 
        public BaseClass Parent { get; set; }
        
        public Group Field { get; set; }

        [Parameter] [JsonIgnore] public string CssClass { get; set; } = "";
        [JsonIgnore] public string CssSelect { get; set; } = "";

        public List<BaseClass> GroupItems { get; set; } = new();

        [Parameter] public BaseClass InstanceClass { get; set; }
        
        public static ModalWindows WindowsRef { get; set; }
        public static ContextMenu ContextMenuRef { get; set; }

        [JsonIgnore] public bool IsEditable = false;

        protected override void OnInitialized() {
            if (InstanceClass != null) {
                Guid = InstanceClass.Guid;
                Title = InstanceClass.Title;
                Field = InstanceClass.Field;
                CssClass = InstanceClass.CssClass;
                CssSelect = InstanceClass.CssSelect;
                GroupItems = InstanceClass.GroupItems;
            }
        }
        
        

        public void SetTitle(string newTitle) {
            this.Title = newTitle;
        }
        


        public virtual void HandleOnDragEnter(BaseClass target) {
            if (target is not IntegrationNode) {
                ((IJSInProcessRuntime)JS).InvokeVoid("ToggleSlots", EventState.LastDomId, false);
                EventState.LastDomId = -1;
            }
            CssClass = "tb-dropborder";
            RenderService.Redraw();
        }

        public virtual void HandleOnDragLeave(BaseClass payload) {
            CssClass = "";
            RenderService.Redraw();
        }

        public virtual void HandleOnDragStart(BaseClass payload) {
            EventState.Payload = payload;
            if (payload is Interface) {
                EventState.DraggingEvent = true;
                var iface = payload as Interface;
                if (iface.Parent.Parent != null && iface.Parent.Parent is IntegrationNode) {
                    EventState.LastDomId = (iface.Parent.Parent as IntegrationNode).DomId;
                    ((IJSInProcessRuntime)JS).InvokeVoid("ToggleSlots", EventState.LastDomId.ToString(), true);
                }
            }

            RenderService.GhostNode.Render();
        }

        public virtual void HandleOnDragEnd() {
            EventState.DraggingEvent = false;
            CssClass = "";
            if (EventState.Payload is Interface) {
                JS.InvokeVoidAsync("ToggleSlots", EventState.LastDomId.ToString(), false);
            }
            EventState.LastDomId = -1;
            RenderService.Redraw();
        }
        
        public virtual void HandleOnDrop() {
            EventState.DraggingEvent = false;
            CssClass = "";
            
            EventState.DeleteItem(EventState.Payload.Guid.ToString());
            GroupItems.Add(EventState.Payload);
            EventState.Payload.Parent = this;
            
            Storage.SaveToSessionStorage();
            RenderService.Redraw();
        }

        public void Render() {
            StateHasChanged();
        }

        /// <summary>
        ///     Helper method to determine class type
        /// </summary>
        /// <param name="Base">Class you want to check</param>
        /// <typeparam name="T">Type to check for</typeparam>
        /// <returns></returns>
        public bool Is<T>(BaseClass Base) {
            return Base is T;
        }

        public bool Is<T>() {
            return this is T;
        }

        public override bool Equals(object obj) {
            if (obj == null || !GetType().Equals(obj.GetType())) return false;

            var obj1 = (BaseClass) obj;
            return obj1.Guid.Equals(Guid);
        }

        public override string ToString() {
            var str = $"Guid: {Guid} Title: {Title} Type: {GetType()}";
            return str;
        }
    }
}