namespace TreeBuilder.ComponentsRedux {
    public partial class GroupField : Group {
        protected override void OnInitialized() {
            Field = this;
            var groupField = Storage.LoadGroupField();
            if (groupField != null) {
                GroupItems.Clear();
                GroupItems = groupField.GroupItems;
                Title = groupField.Title;
            }
        }
    }
}