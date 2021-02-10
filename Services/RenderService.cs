using System;
using System.Threading.Tasks;
using TreeBuilder.Classes;
using TreeBuilder.ComponentsRedux;
using TreeBuilder.Pages;

namespace TreeBuilder.Services {
    public class RenderService {
        public IntegrationField IntegrationField { get; set; }
        public GroupField GroupField { get; set; }
        public IntegrationGhostNode GhostNode { get; set; }
        public Pages.Index App { get; set; }

        public enum Element {
            All = 0,
            IntegrationField = 1,
            GroupField = 2,
            GhostNode = 4,
            App = 8
        }
        
        public void Redraw(Element e = 0) {
            if (e == 0) {
                IntegrationField.Render();
                GroupField.Render();
                GhostNode.Render();
                App.Render();
                return;
            }
            
            if (((int)e & 1) > 0) {
                IntegrationField.Render();
            }
            if (((int)e & 2) > 0) {
                GroupField.Render();
            }
            if (((int)e & 4) > 0) {
                GhostNode.Render();
            }
            if (((int)e & 8) > 0) {
                App.Render();
            }
        }

        public void RedrawObject(BaseClass obj) {
            obj.Render();
        }
        
    }
}