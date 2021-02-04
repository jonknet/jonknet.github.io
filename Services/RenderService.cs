using System.Threading.Tasks;
using TreeBuilder.ComponentsRedux;

namespace TreeBuilder.Services {
    public class RenderService {
        public IntegrationField IntegrationField { get; set; }
        public GroupField GroupField { get; set; }
        public IntegrationGhostNode GhostNode { get; set; }
        public async Task Redraw() {
            await IntegrationField.Render();
            await GroupField.Render();
            await GhostNode.Render();
        }
    }
}