using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using TreeBuilder.Classes;
using TreeBuilder.Services;

namespace TreeBuilder.ComponentsRedux {
    public partial class GroupField : Group {

        protected override void OnInitialized() {
            GroupField groupField = Storage.LoadGroupField();
            if (groupField != null) {
                GroupItems.Clear();
                GroupItems = groupField.GroupItems;
                Title = groupField.Title;
            }
        }
    }
}