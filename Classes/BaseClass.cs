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
        [CascadingParameter(Name = "Field")] public Group Field { get; set; } = null;
        public string CssClass { get; set; } = "";
        public string CssSelect { get; set; } = "";
        
        public static BaseClass Payload { get; set; } = null;
        public static BaseClass Selection { get; set; } = null;

        [Parameter] public List<BaseClass> GroupItems { get; set; } = new List<BaseClass>();

        public BaseClass() {
            
        }
        public BaseClass(BaseClass Parent, Group Field) {
            this.Parent = Parent;
            this.Field = Field;
        }

        public virtual void HandleOnDragEnter(){
            CssClass = "tb-dropborder";
        }

        public virtual void HandleOnDragLeave(){
            CssClass = "";
        }

        public virtual void HandleOnDragStart(BaseClass payload){
            Payload = payload;
            Console.WriteLine(payload.Guid);
        }

        public virtual void HandleOnDragEnd(){
            CssClass = "";
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