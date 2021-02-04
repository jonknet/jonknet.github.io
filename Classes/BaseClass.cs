using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using TreeBuilder.ComponentsRedux;
using TreeBuilder.Services;

namespace TreeBuilder.Classes {
    public class BaseClass : ComponentBase {
        [Inject] protected StorageService Storage { get; set; }
        [Inject] protected RenderService RenderService { get; set; }

        [Parameter] public Guid Guid { get; set; } = Guid.NewGuid();
        [Parameter] public string Title { get; set; } = "";
        [Parameter] [JsonIgnore] public BaseClass Parent { get; set; } = null;

        [CascadingParameter(Name = "Field")] [JsonIgnore] public Group Field { get; set; } = null;

        [JsonIgnore] public string CssClass { get; set; } = "";
        [JsonIgnore] public string CssSelect { get; set; } = "";

        [Parameter] public List<BaseClass> GroupItems { get; set; } = new();

        public BaseClass() { }

        public BaseClass(BaseClass Parent, Group Field) {
            this.Parent = Parent;
            this.Field = Field;
        }

        public virtual void HandleOnDragEnter() {
            CssClass = "tb-dropborder";
        }

        public virtual void HandleOnDragLeave() {
            CssClass = "";
        }

        public virtual void HandleOnDragStart(BaseClass payload) {
            EventState.Payload = payload;
        }

        public virtual void HandleOnDragEnd() {
            CssClass = "";
            EventState.DraggingEvent = false;
            RenderService.Redraw();
        }

        public virtual void HandleOnDrop() {
            Storage.SaveToSessionStorage();
        }

        public virtual void Render() {
            StateHasChanged();
        }

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