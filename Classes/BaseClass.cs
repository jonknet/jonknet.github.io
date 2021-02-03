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
        [Parameter] public BaseClass Parent { get; set; } = null;
        [CascadingParameter] public Group Field { get; set; } = null;
        public string CssClass { get; set; } = "";
        public string CssSelect { get; set; } = "";
        
        public static BaseClass Payload { get; set; } = null;
        public static BaseClass Selection { get; set; } = null;

        [Parameter] public List<BaseClass> GroupItems { get; set; } = new List<BaseClass>();

        public BaseClass() {
            Guid = Guid.NewGuid();
            Title = "";
            Parent = null;
            Field = null;
            CssClass = "";
            CssSelect = "";
            Payload = null;
            Selection = null;
        }

        public virtual void HandleOnDragEnter(){
            CssClass = "tb-dropborder";
        }

        public virtual void HandleOnDragLeave(){
            CssClass = "";
        }

        public virtual void HandleOnDragStart(BaseClass payload){
            Payload = payload;
        }

        public virtual void HandleOnDragEnd(){
            CssClass = "";
            Payload = null;
        }
        
        public virtual void HandleOnDrop() {
            Storage.SaveToSessionStorage();
        }

        public virtual void Render() {
            StateHasChanged();
        }

        public override bool Equals(object obj)
        {
            if(obj == null || ! this.GetType().Equals(obj.GetType()))
            {
                return false;
            }

            BaseClass obj1 = (BaseClass) obj;
            return (obj1.Guid.Equals(this.Guid));
        }

        public override string ToString()
        {
            string str = $"Guid: {Guid}\r\nTitle: {Title}\r\n";
            return str;
        }

    }
}