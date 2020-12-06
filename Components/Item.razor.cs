using Microsoft.AspNetCore.Components;
using System;
using TreeBuilder.Services;

namespace TreeBuilder.Components {
    public partial class Item : ComponentBase {

        [Inject]
        ComponentTracker componentTracker { get; set; }

        [Parameter]
        public string Title { get; set; } = "Default Title";

        public Guid Uid { get; set; }

        public string CssClass { get; set; } = "";

        protected override void OnInitialized(){
            Uid = Guid.NewGuid();
            componentTracker.Add(Uid,this);
        }

        public void HandleOnInput(ChangeEventArgs args){
            Title = args.Value.ToString();
        }  
    }
}