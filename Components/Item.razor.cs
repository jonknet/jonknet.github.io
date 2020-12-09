using Microsoft.AspNetCore.Components;
using System;
using TreeBuilder.Services;
using System.Collections.Generic;

namespace TreeBuilder.Components {

    public class Item : ComponentBase {

        [Parameter]
        public string Title { get; set; } = "Default Title";

        [Parameter]
        public Guid Uid { get; set; } = Guid.NewGuid();

        [Parameter]
        public Group Parent { get; set; } = null;

        public string CssClass { get; set; } = "";

        public static Item Payload { get; set; } = null;

        public void HandleOnInput(ChangeEventArgs args){
            Title = args.Value.ToString();
        }  

        public void HandleOnDragEnter(){
            CssClass = "tb-dropborder";
        }

        public void HandleOnDragLeave(){
            CssClass = "";
        }

        public void HandleOnDragStart(Item payload){
            Payload = payload;
        }

        public void HandleOnDragEnd(){
            CssClass = "";  
        }

        public override bool Equals(object obj)
        {
            return (Uid == (obj as Item).Uid);
        }

    }
}