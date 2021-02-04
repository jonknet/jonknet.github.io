using System;
using TreeBuilder.Classes;

namespace TreeBuilder.ComponentsRedux {
    public partial class Interface : BaseClass {
        public Interface() { }

        public Interface(BaseClass Parent, Group Field) : base(Parent, Field) { }

        protected override void OnInitialized() { }

        public override void HandleOnDragStart(BaseClass Payload) {
            EventState.Payload = Payload;
            // Set DraggingEvent to signal 1) Ghost Node should appear and 2) Additional interface slots may appear
            EventState.DraggingEvent = true;
            RenderService.Redraw();
        }

    }
}