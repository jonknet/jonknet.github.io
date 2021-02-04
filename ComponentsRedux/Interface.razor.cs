using System;
using System.Threading.Tasks;
using TreeBuilder.Classes;
using System.Timers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace TreeBuilder.ComponentsRedux {
    public partial class Interface : BaseClass {
        
        public Interface() { }

        public Interface(BaseClass Parent, Group Field) : base(Parent, Field) { }

        protected override void OnInitialized() { }

        public override void HandleOnDragStart(BaseClass Payload) {
            EventState.Payload = Payload;
            EventState.DraggingEvent = true;
            RenderService.Redraw();
            JS.InvokeVoidAsync("UnhideAdditionalSlots");
        }

    }
}