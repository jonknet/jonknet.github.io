using System;
using System.Runtime.InteropServices;
using TreeBuilder.Services;
using Microsoft.AspNetCore.Components;

namespace TreeBuilder.Components {
    public partial class Trash : Item {

        public override void HandleOnDrop() {
            Console.WriteLine(Payload);
            
            // Hack for ghost integration node, once again
            if (Payload.GetType() == typeof(Interface)) {
                (Payload as Interface).HandleOnDragEnd();
            }
            //End Hack
            
            Payload.Parent.Items.Remove(Payload);
            while (Payload.Parent != null) {
                Payload = Payload.Parent;
            }

            (Payload as Field).Redraw();

            base.HandleOnDrop();
        }
    }
}