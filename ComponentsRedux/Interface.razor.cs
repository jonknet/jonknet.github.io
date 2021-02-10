using System;
using TreeBuilder.Classes;

namespace TreeBuilder.ComponentsRedux {
    public partial class Interface : BaseClass{
        public Interface() { }

        public Interface(BaseClass Parent) : base(Parent, Parent as Group) { }


    }
}