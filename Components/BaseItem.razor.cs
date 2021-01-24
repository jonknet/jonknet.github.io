using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace TreeBuilder.Components {

    public enum CLASS_TYPE
    {
        BASECLASS,
        FIELD,
        GROUP,
        INTEGRATIONFIELD,
        INTEGRATIONNODE,
        INTEGRATIONSLOT,
        INTERFACE
    }

    public partial class BaseItem : ComponentBase
    {

        [Inject] private Blazored.LocalStorage.ILocalStorageService LocalStorageService { get; set; }
        [Parameter] public string Title { get; set; } = "Default Title";
        [Parameter] public Guid Uid { get; set; } = Guid.NewGuid();
        [Parameter][JsonIgnore] public Group Parent { get; set; } = null;
        [CascadingParameter][JsonIgnore] public Field Field { get; set; } = null;
        [Parameter] public string Name { get; set; } = "";
        [Parameter] public int Index { get; set; }
        [Parameter] public Interface Iface { get; set; } = null;
        [Parameter][JsonIgnore] public EventCallback SaveToLocalStorageCallback { get; set; }
        [Parameter] public BaseItem Instance { get; set; }
        [JsonIgnore] public string CssClass { get; set; } = "";
        [JsonIgnore] public string CssSelect { get; set; } = "";
        [JsonIgnore] public bool renameModal { get; set; } = false;
        public CLASS_TYPE ClassType { get; set; } = CLASS_TYPE.BASECLASS;

        [Parameter] public List<BaseItem> Items { get; set; }= new List<BaseItem>();

        public static BaseItem Payload { get; set; } = null;
        public static BaseItem Selection { get; set; } = null;
        
        protected override void OnInitialized()
        {
            
        }

        public virtual void HandleOnInput(ChangeEventArgs args){
            Title = args.Value.ToString();
            
        }  

        public virtual void HandleOnDragEnter(){
            CssClass = "tb-dropborder";
        }

        public virtual void HandleOnDragLeave(){
            CssClass = "";
        }

        public virtual void HandleOnDragStart(BaseItem payload){
            Payload = payload;
        }

        public virtual void HandleOnDragEnd(){
            CssClass = "";
            Payload = null;
        }

        public virtual async void HandleOnDrop()
        {
            await SaveToLocalStorageCallback.InvokeAsync();
        }

        public void SaveLocalState()
        {
            
        }

        public void SelectAction(){
            // If in an IntegrationNode, don't select anything
            if(Field.GetType() == typeof(IntegrationField))
                return;
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
            Field.Redraw();
        }

        public void TriggerRenameModal(){
            if (renameModal) {
                
                if (this.GetType() == typeof(Interface)) {
                    foreach (var item in Parent.Items) {
                        if (item.Iface != null && item.Iface.Uid == this.Uid) {
                            item.Iface.Title = Title;
                        }
                    }   
                } 
                foreach (var item in Parent.Items) {
                    if (item.Uid == this.Uid) {
                        item.Title = Title;
                    }
                }   
                SaveToLocalStorageCallback.InvokeAsync();
            }

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

        public void Redraw()
        {
            StateHasChanged();
        }

        public override bool Equals(object obj) {
            BaseItem item = obj as BaseItem;
            if (item == null) {
                return false;
            }
            return (Uid == item.Uid);
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

            if (Items != null)
            {
                output += ("Items:{");
                foreach (var item in Items)
                {
                    output += ($"{item},");
                }
            }
            return output;
        }
    }
}