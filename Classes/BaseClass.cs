using System;
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
using System.Diagnostics;
using System.Reflection;

namespace TreeBuilder.Classes {
    /// <summary>
    ///     BaseClass of all types
    /// </summary>
    public class BaseClass : ComponentBase, IDisposable {

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
        
        [JsonProperty(IsReference = true)] 
        public Group Field { get; set; }

        [Parameter] [JsonIgnore] public string CssClass { get; set; } = "";
        [JsonIgnore] public string CssSelect { get; set; } = "";

        [JsonProperty(IsReference = true)]
        public List<BaseClass> GroupItems { get; set; } = new();

        [Parameter] public BaseClass InstanceClass { get; set; }
        
        public static ModalWindows WindowsRef { get; set; }
        public static ContextMenu ContextMenuRef { get; set; }

        [JsonIgnore] public bool IsEditable = false;
        
        #if DEBUG
        private static Stopwatch _stopwatch = new Stopwatch();
        #endif

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

        public virtual void HandleOnDragEnter(BaseClass target) {
            #if DEBUG
                _stopwatch.Reset();
                _stopwatch.Start();
            #endif
            if (target is not IntegrationNode) {
                ((IJSInProcessRuntime)JS).InvokeVoid("ToggleSlots", EventState.LastDomId, false);
                EventState.LastDomId = -1;
            }
            CssClass = "tb-dropborder";
            //RenderService.Redraw();
            #if DEBUG
                _stopwatch.Stop();
                Console.WriteLine(GetType().Name + ":" + MethodBase.GetCurrentMethod().Name + ":" + _stopwatch.ElapsedMilliseconds);
            #endif
        }

        public virtual void HandleOnDragLeave(BaseClass payload) {
            #if DEBUG
                        _stopwatch.Reset();
                        _stopwatch.Start();
            #endif
            CssClass = "";
            //RenderService.Redraw();
            #if DEBUG
                        _stopwatch.Stop();
                        Console.WriteLine(GetType().Name + ":" + MethodBase.GetCurrentMethod().Name + ":" + _stopwatch.ElapsedMilliseconds);
            #endif
        }

        public virtual void HandleOnDragStart(BaseClass payload) {
            #if DEBUG
                        _stopwatch.Reset();
                        _stopwatch.Start();
            #endif
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
            #if DEBUG
                        _stopwatch.Stop();
                        Console.WriteLine(GetType().Name + ":" + MethodBase.GetCurrentMethod().Name + ":" + _stopwatch.ElapsedMilliseconds);
            #endif
        }

        public virtual void HandleOnDragEnd() {
            #if DEBUG
                        _stopwatch.Reset();
                        _stopwatch.Start();
            #endif
            EventState.DraggingEvent = false;
            CssClass = "";
            if (EventState.Payload is Interface) {
                ((IJSInProcessRuntime)JS).InvokeVoid("ToggleSlots", EventState.LastDomId.ToString(), false);
            }
            EventState.LastDomId = -1;
            RenderService.Redraw(RenderService.Element.GhostNode);
            #if DEBUG
                        _stopwatch.Stop();
                        Console.WriteLine(GetType().Name + ":" + MethodBase.GetCurrentMethod().Name + ":" + _stopwatch.ElapsedMilliseconds);
            #endif
        }
        
        public virtual void HandleOnDrop() {
            #if DEBUG
                        _stopwatch.Reset();
                        _stopwatch.Start();
            #endif
            EventState.DraggingEvent = false;
            CssClass = "";
            
            EventState.DeleteItem(EventState.Payload);
            GroupItems.Add(EventState.Payload);
            
            EventState.Payload.Parent = this;
            EventState.Payload.Field = Field;
            
            //Storage.SaveToSessionStorage();
            RenderService.Redraw(RenderService.Element.GhostNode | RenderService.Element.GroupField | RenderService.Element.IntegrationField);
            #if DEBUG
                        _stopwatch.Stop();
                        Console.WriteLine(GetType().Name + ":" + MethodBase.GetCurrentMethod().Name + ":" + _stopwatch.ElapsedMilliseconds);
            #endif
        }

        public void Render() {
            StateHasChanged();
        }

        /// <summary>
        /// Easy way to clean up from the Runtime dictionaries
        /// </summary>
        public void Dispose() {
            var g = EventState.RuntimeGroups;
            var i = EventState.RuntimeIntegrations;
            var ifc = EventState.RuntimeInterfaces;
            Console.WriteLine("Disposed " + Guid);
            if (GetType() == typeof(Group)) {
                g.Remove(this.Guid);
            } else if (GetType() == typeof(IntegrationNode)) {
                i.Remove(this.Guid);
            } else if (GetType() == typeof(Interface)) {
                ifc.Remove(this.Guid);
            }
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