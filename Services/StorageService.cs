using Blazored.LocalStorage;
using Newtonsoft.Json;
using TreeBuilder.ComponentsRedux;

namespace TreeBuilder.Services {
    public class StorageService {
        
        public GroupField GroupField;
        public IntegrationField IntegrationField;
        
        public ISyncLocalStorageService LocalStorageService;

        public JsonSerializerSettings settings = new()
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            NullValueHandling = NullValueHandling.Include,
            TypeNameHandling = TypeNameHandling.All,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize
        };

        public StorageService(ISyncLocalStorageService LocalStorageService) {
            this.LocalStorageService = LocalStorageService;

            GroupField = LoadGroupField();
            IntegrationField = LoadIntegrationField();
        }

        /// <summary>
        /// Saves both IntegrationField and GroupField to Local Storage in the Browser as JSON
        /// </summary>
        public void SaveToSessionStorage() {
            LocalStorageService.SetItem("TreeBuilder_IntegrationField",
                JsonConvert.SerializeObject(IntegrationField, settings));
            LocalStorageService.SetItem("TreeBuilder_GroupField",
                JsonConvert.SerializeObject(GroupField, settings));
        }

        /// <summary>
        /// Loads Group Field from JSON
        /// </summary>
        /// <returns>GroupField or, if none, an empty one</returns>
        public GroupField LoadGroupField()
        {
            if (!LocalStorageService.ContainKey("TreeBuilder_GroupField")) return new GroupField();
            return JsonConvert.DeserializeObject<GroupField>(
                LocalStorageService.GetItemAsString("TreeBuilder_GroupField"), settings);
        }

        /// <summary>
        /// Loads Integration Field from JSON
        /// </summary>
        /// <returns>IntegrationField or, if none, a empty one</returns>
        public IntegrationField LoadIntegrationField() {
            if (!LocalStorageService.ContainKey("TreeBuilder_IntegrationField")) return new IntegrationField();
            return JsonConvert.DeserializeObject<IntegrationField>(
                LocalStorageService.GetItemAsString("TreeBuilder_IntegrationField"), settings);
        }

        
        public void SaveValue<T>(string Key, T Value) {
            LocalStorageService.SetItem(Key, Value);
        }

        public T LoadValue<T>(string Key) {
            return LocalStorageService.GetItem<T>(Key);
        }

        public void ClearAllKeys() {
            LocalStorageService.Clear();
        }
    }
}