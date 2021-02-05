using TreeBuilder.Classes;

namespace TreeBuilder.ComponentsRedux {
    public partial class Interface : BaseClass {
        public Interface() { }

        public Interface(BaseClass Parent, Group Field) : base(Parent, Field) { }
    }
}