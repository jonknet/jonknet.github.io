using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using TreeBuilder.ComponentsRedux;
using TreeBuilder.Services;

namespace TreeBuilder.Classes {
    /// <summary>
    ///     BaseClass of all types
    /// </summary>
    public class BaseClass : ComponentBase {
        public BaseClass() {
            Guid = Guid.NewGuid();
            Title = "Default";
        }

        public BaseClass(BaseClass Parent, Group Field) {
            this.Parent = Parent;
            this.Field = Field;
            Guid = Guid.NewGuid();
            Title = "Default";
        }


        [Inject] protected StorageService Storage { get; set; }
        [Inject] protected RenderService RenderService { get; set; }
        [Inject] protected IJSRuntime JS { get; set; }

        [Parameter] public Guid Guid { get; set; } = Guid.NewGuid();
        [Parameter] public string Title { get; set; } = "Default";
        [Parameter] [JsonIgnore] public BaseClass Parent { get; set; }

        [CascadingParameter(Name = "Field")]
        [JsonIgnore]
        public Group Field { get; set; }

        [Parameter] [JsonIgnore] public string CssClass { get; set; } = "";
        [JsonIgnore] public string CssSelect { get; set; } = "";

        [Parameter] public List<BaseClass> GroupItems { get; set; } = new();

        public bool IsEditable = false;

        protected override void OnInitialized() { }

        public virtual void HandleOnDragEnter() {
            CssClass = "tb-dropborder";
            RenderService.Redraw();
        }

        public virtual void HandleOnDragLeave() {
            Console.WriteLine("HandleOnDragLeave");
            CssClass = "";
            RenderService.Redraw();
        }

        public virtual void HandleOnDragStart(BaseClass payload) {
            Console.WriteLine("HandleOnDragStart");
            EventState.Payload = payload;
            EventState.DraggingEvent = true;
            if (payload is Interface) {
                var iface = payload as Interface;
                if (iface.Parent.Parent != null && iface.Parent.Parent is IntegrationNode) {
                    EventState.LastDomId = (iface.Parent.Parent as IntegrationNode).DomId;
                    JS.InvokeVoidAsync("ToggleSlots", EventState.LastDomId.ToString(), true);
                }
            }

            RenderService.Redraw();
        }

        public virtual void HandleOnDragEnd() {
            Console.WriteLine("HandleOnDragEnd");
            EventState.DraggingEvent = false;
            CssClass = "";
            JS.InvokeVoidAsync("ToggleSlots", EventState.LastDomId.ToString(), false);
            RenderService.Redraw();
        }

        public virtual void HandleOnChange(ChangeEventArgs e) {
            Console.WriteLine(e.Value.ToString());
        }

        public virtual void HandleOnDrop() {
            Storage.SaveToSessionStorage();
        }

        public virtual async Task Render() {
            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        ///     Helper method to determine class type
        /// </summary>
        /// <param name="Base">Class you want to check</param>
        /// <typeparam name="T">Type to check for</typeparam>
        /// <returns></returns>
        public static bool Is<T>(BaseClass Base) {
            return Base is T;
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