using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Telerik.Blazor.Components;
using TreeBuilder.ComponentsRedux;
using TreeBuilder.Services;

namespace TreeBuilder.Classes {
    /// <summary>
    ///     BaseClass of all types
    /// </summary>
    public class BaseClass : ComponentBase {
        public BaseClass() {
            
        }

        public BaseClass(BaseClass Parent, Group Field) {
            this.Parent = Parent;
            this.Field = Field;
        }
        
        [Inject] protected StorageService Storage { get; set; }
        [Inject] protected RenderService RenderService { get; set; }
        [Inject] protected IJSRuntime JS { get; set; }
        [Inject] protected EventState EventState { get; set; }

        [Parameter] public Guid Guid { get; set; } = Guid.NewGuid();
        [Parameter] public string Title { get; set; } = "Default";
        [Parameter] public BaseClass Parent { get; set; }

        [CascadingParameter(Name = "Field")]
        public Group Field { get; set; }

        [Parameter] [JsonIgnore] public string CssClass { get; set; } = "";
        [JsonIgnore] public string CssSelect { get; set; } = "";

        [Parameter] public List<BaseClass> GroupItems { get; set; } = new();
        
        public static ModalWindows WindowsRef { get; set; }
        public static ContextMenu ContextMenuRef { get; set; }

        public bool IsEditable = false;

        protected override void OnInitialized() {
            #if DEBUG
                Console.WriteLine(JsonConvert.SerializeObject(this,
                    new JsonSerializerSettings(){
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    PreserveReferencesHandling = PreserveReferencesHandling.All}));
            #endif
        }

        public void SetTitle(string newTitle) {
            Title = newTitle;
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
            RenderService.Redraw();
        }

        public virtual void HandleOnDragEnd() {
            EventState.DraggingEvent = false;
            CssClass = "";
            if (EventState.Payload is Interface) {
                ((IJSInProcessRuntime)JS).InvokeVoid("ToggleSlots", EventState.LastDomId.ToString(), false);
            }
            EventState.LastDomId = -1;
            RenderService.Redraw();
        }
        
        public virtual void HandleOnDrop() {
            EventState.DraggingEvent = false;
            CssClass = "";
            Storage.SaveToSessionStorage();
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
            var str = $"Guid: {Guid}\r\nTitle: {Title}\r\n";
            return str;
        }
    }
}