using System;

namespace TreeBuilder.Components {
    public class FieldOutput : Field {

        public override void HandleOnDrop(){
            if(Payload.Parent != this){
                Console.WriteLine(Payload.Uid);
            }
            CssClass = "";
        }
    }
}