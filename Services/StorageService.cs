using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using TreeBuilder.ComponentsRedux;

namespace TreeBuilder.Services {
    public class StorageService {

        public ISyncLocalStorageService LocalStorageService;

        public IntegrationField IntegrationField;
        public GroupField GroupField;

        public JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            NullValueHandling = NullValueHandling.Include,
            TypeNameHandling = TypeNameHandling.All,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public StorageService(ISyncLocalStorageService LocalStorageService) {
            this.LocalStorageService = LocalStorageService;
        }

        public void Save(IntegrationField intField, GroupField grpField)
        {
            LocalStorageService.Clear();
            LocalStorageService.SetItem<string>("TreeBuilder_IntegrationField", JsonConvert.SerializeObject(intField, settings));
            LocalStorageService.SetItem<string>("TreeBuilder_GroupField", JsonConvert.SerializeObject(grpField, settings));
        }
        
        public void SaveToSessionStorage() {
            LocalStorageService.Clear();
            LocalStorageService.SetItem<string>("TreeBuilder_IntegrationField", JsonConvert.SerializeObject(IntegrationField, settings));
            LocalStorageService.SetItem<string>("TreeBuilder_GroupField", JsonConvert.SerializeObject(GroupField, settings));
        }

        public GroupField LoadGroupField() {
            if (!LocalStorageService.ContainKey("TreeBuilder_GroupField")) {
                return null;
            }
            return JsonConvert.DeserializeObject<GroupField>(LocalStorageService.GetItemAsString("TreeBuilder_GroupField"),settings);
        }

        public IntegrationField LoadIntegrationField() {
            if (!LocalStorageService.ContainKey("TreeBuilder_IntegrationField")) {
                return null;
            }
            return JsonConvert.DeserializeObject<IntegrationField>(LocalStorageService.GetItemAsString("TreeBuilder_IntegrationField"),settings);
        }
    }
}