﻿using System;
using System.Collections.Generic;
using Blazored.LocalStorage;
using Newtonsoft.Json;
using TreeBuilder.Classes;
using TreeBuilder.ComponentsRedux;

namespace TreeBuilder.Services {
    public class StorageService {
        public GroupField GroupField;

        public IntegrationField IntegrationField;
        public ISyncLocalStorageService LocalStorageService;

        public JsonSerializerSettings settings = new()
        {
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            NullValueHandling = NullValueHandling.Include,
            TypeNameHandling = TypeNameHandling.All,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public StorageService(ISyncLocalStorageService LocalStorageService) {
            this.LocalStorageService = LocalStorageService;
        }

        public void Save(IntegrationField intField, GroupField grpField) {
            LocalStorageService.Clear();
            LocalStorageService.SetItem("TreeBuilder_IntegrationField",
                JsonConvert.SerializeObject(intField, settings));
            LocalStorageService.SetItem("TreeBuilder_GroupField",
                JsonConvert.SerializeObject(grpField, settings));
            
        }

        public void SaveToSessionStorage() {
            LocalStorageService.Clear();
            LocalStorageService.SetItem("TreeBuilder_IntegrationField",
                JsonConvert.SerializeObject(IntegrationField, settings));
            LocalStorageService.SetItem("TreeBuilder_GroupField",
                JsonConvert.SerializeObject(GroupField, settings));
       
        }

        public GroupField LoadGroupField() {
            if (!LocalStorageService.ContainKey("TreeBuilder_GroupField")) return null;
            return JsonConvert.DeserializeObject<GroupField>(
                LocalStorageService.GetItemAsString("TreeBuilder_GroupField"), settings);
        }

        public IntegrationField LoadIntegrationField() {
            if (!LocalStorageService.ContainKey("TreeBuilder_IntegrationField")) return null;
            return JsonConvert.DeserializeObject<IntegrationField>(
                LocalStorageService.GetItemAsString("TreeBuilder_IntegrationField"), settings);
        }
        
    }
}