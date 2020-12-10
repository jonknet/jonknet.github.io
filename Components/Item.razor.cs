using Microsoft.AspNetCore.Components;
using System;
using TreeBuilder.Services;
using System.Collections.Generic;

namespace TreeBuilder.Components {

    public partial class Item : ComponentBase {

        [Parameter]
        public string Title { get; set; } = "Default Title";

        [Parameter]
        public Guid Uid { get; set; } = Guid.NewGuid();

        [Parameter]
        public Group Parent { get; set; } = null;

        [Parameter]
        public Item Instance { get; set; } = null;

        [CascadingParameter]
        public Field Field { get; set; } = null;

        public string CssClass { get; set; } = "";

        public string CssSelect { get; set; } = "";

        public static Item Payload { get; set; } = null;

        public static Item Selection { get; set; } = null;

        public bool renameModal { get; set; } = false;

        protected override void OnInitialized()
        {
            
        }

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

        public void SelectAction(){
            // For current selection, we have to unselect
            if(Selection != null)
                Selection.CssSelect = "";
            if(Selection != null && Selection == this){
                // If selection is the same, deselect
                Selection = null;
            } else {
                CssSelect = "tb-dropborder";
                Selection = this;
            }
            Field.Refresh();
        }

        public void TriggerRenameModal(){
            if(renameModal)
                Instance.Title = Title;
            renameModal = !renameModal;
        }
/*        public void Rename(Item payload){
            Payload = payload;
            if(renameModal){
                FindItem(this,payload).Title = this.Title;
            }
            renameModal = !renameModal;
        }

        public Item FindItem(Item start, Item target){
            if(start == target)
                return start;
            foreach(Item item in start.Items){
                if(item == target){
                    return item;
                } else {
                    return FindItem(item, target);
                }
            }
            return null;
        }
*/
        public override bool Equals(object obj)
        {
            return (Uid == (obj as Item).Uid);
        }

        public override string ToString()
        {
            var output = ($"Item=> Uid:{Uid} Title:{Title} ");
            if(Parent != null){
                output += ($"Parent:{Parent.Uid} ");
            }
            if(Selection != null){
                output += ($"Selection:{Selection.Uid} ");
            }
            if(Payload != null){
                output += ($"Payload:{Payload.Uid} \n");
            }
            return output;
        }

    }
}