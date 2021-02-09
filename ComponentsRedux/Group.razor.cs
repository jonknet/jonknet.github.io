using System;
using TreeBuilder.Classes;

namespace TreeBuilder.ComponentsRedux {
    public partial class Group : BaseClass {
        public Group() { }
        public Group(BaseClass Parent) : base(Parent, Parent as Group) { }
        
        public override void HandleOnDrop()
        {
            if ((!EventState.Payload.Is<Interface>() && !EventState.Payload.Is<Group>()) ||
                GroupItems.Contains(EventState.Payload) ||
                EventState.Payload == this || EventState.Payload.Is<IntegrationNode>())
            {
                HandleOnDragEnd();
                return;
            }

            base.HandleOnDrop();
        }

        /// <summary>
        /// Pass-thru functionality so we can call BaseClass's HandleOnDrop from IntegrationField
        /// </summary>
        protected void BaseOnDrop()
        {
            base.HandleOnDrop();
        }
        
    }
}