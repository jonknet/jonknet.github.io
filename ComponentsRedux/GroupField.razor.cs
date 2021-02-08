using System;
using TreeBuilder.Classes;

namespace TreeBuilder.ComponentsRedux {
    public partial class GroupField : Group {
        protected override void OnInitialized() {
            Field = Storage.GroupField;
            GroupItems = Storage.GroupField.GroupItems;
        }
    }
}