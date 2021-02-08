using System;
using System.Threading.Tasks;
using TreeBuilder.ComponentsRedux;
using TreeBuilder.Pages;

namespace TreeBuilder.Services {
    public class RenderService {
        public IntegrationField IntegrationField { get; set; }
        public GroupField GroupField { get; set; }
        public IntegrationGhostNode GhostNode { get; set; }
        public Pages.Index App { get; set; }

        public void Redraw() {
            IntegrationField.Render();
            GroupField.Render();
            GhostNode.Render();
            App.Render();
        }
    }
}