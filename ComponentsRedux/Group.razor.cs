using TreeBuilder.Classes;

namespace TreeBuilder.ComponentsRedux {
    public partial class Group : BaseClass {
        public Group() { }
        public Group(BaseClass Parent, Group Field) : base(Parent, Field) { }
        

        public override void HandleOnDrop() {
            if (Is<IntegrationNode>(EventState.Payload) || EventState.Payload.Field != Field ||
                EventState.Payload == this ||
                EventState.Payload.Parent == this) return;

            GroupItems.Add(EventState.Payload);

            EventState.Payload.Parent.GroupItems.Remove(EventState.Payload);

            EventState.Payload.Parent = this;

            EventState.Payload.Field = Field;

            RenderService.Redraw();

            base.HandleOnDrop();
        }

        public override string ToString() {
            var str = "Group: \r\n" + base.ToString() + "Items: \r\n";
            foreach (var item in GroupItems) str += $"{item}\r\n";
            return str;
        }
    }
}