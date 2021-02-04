using TreeBuilder.ComponentsRedux;

namespace TreeBuilder.Services {
    public class RenderService {
        public IntegrationField IntegrationField { get; set; }
        public GroupField GroupField { get; set; }
        public IntegrationGhostNode GhostNode { get; set; }
        public void Redraw() {
            IntegrationField.Render();
            GroupField.Render();
            GhostNode.Render();
        }
    }
}