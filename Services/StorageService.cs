using System.Threading.Tasks;
using Blazored.LocalStorage;
using Newtonsoft.Json;
using TreeBuilder.ComponentsRedux;

namespace TreeBuilder.Services {
    public class StorageService {
        
        public GroupField GroupField;
        public IntegrationField IntegrationField;
        
        public ILocalStorageService LocalStorageService;
        public ISyncLocalStorageService LocalStorageSyncService;

        public JsonSerializerSettings settings = new()
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            NullValueHandling = NullValueHandling.Include,
            TypeNameHandling = TypeNameHandling.All,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize
        };

        public StorageService(ILocalStorageService LocalStorageService,ISyncLocalStorageService LocalStorageSyncService) {
            this.LocalStorageService = LocalStorageService;
            this.LocalStorageSyncService = LocalStorageSyncService;
            GroupField = LoadGroupField();
            IntegrationField = LoadIntegrationField();
        }

        /// <summary>
        /// Saves both IntegrationField and GroupField to Local Storage in the Browser as JSON
        /// </summary>
        public async void SaveToSessionStorage() {
            await LocalStorageService.SetItemAsync("TreeBuilder_IntegrationField",
                JsonConvert.SerializeObject(IntegrationField, settings));
            await LocalStorageService.SetItemAsync("TreeBuilder_GroupField",
                JsonConvert.SerializeObject(GroupField, settings));
        }

        /// <summary>
        /// Loads Group Field from JSON
        /// </summary>
        /// <returns>GroupField or, if none, an empty one</returns>
        public GroupField LoadGroupField()
        {
            if (!LocalStorageSyncService.ContainKey("TreeBuilder_GroupField")) return new GroupField();
            return JsonConvert.DeserializeObject<GroupField>(
                LocalStorageSyncService.GetItemAsString("TreeBuilder_GroupField"), settings);
        }

        /// <summary>
        /// Loads Integration Field from JSON
        /// </summary>
        /// <returns>IntegrationField or, if none, a empty one</returns>
        public IntegrationField LoadIntegrationField() {
            if (!LocalStorageSyncService.ContainKey("TreeBuilder_IntegrationField")) return new IntegrationField();
            return JsonConvert.DeserializeObject<IntegrationField>(
                LocalStorageSyncService.GetItemAsString("TreeBuilder_IntegrationField"), settings);
        }

        
        public void SaveValue<T>(string Key, T Value) {
            LocalStorageSyncService.SetItem(Key, Value);
        }

        public T LoadValue<T>(string Key) {
            return LocalStorageSyncService.GetItem<T>(Key);
        }

        public void ClearAllKeys() {
            LocalStorageSyncService.Clear();
        }
    }
}