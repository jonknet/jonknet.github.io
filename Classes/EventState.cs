using TreeBuilder.ComponentsRedux;

namespace TreeBuilder.Classes {
    public static class EventState {
        public static BaseClass Payload;
        public static BaseClass Selection;

        public static InterfaceSlot RightTop;
        public static InterfaceSlot RightBottom;
        
        public static bool DraggingEvent = false;
    }
}