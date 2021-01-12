using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using TreeBuilder.Services;
using Newtonsoft.Json;


namespace TreeBuilder.Components {
    public partial class Field : Group
    {
        [Inject] private ILocalStorageService LocalStorageService { get; set; }

        public Field()
        {
            ClassType = CLASS_TYPE.FIELD;
        }

        protected override async Task OnInitializedAsync()
        {
            if (await LocalStorageService.ContainKeyAsync("TreeBuilder_GroupField"))
            {
                Field _if = JsonConvert.DeserializeObject<Field>(await LocalStorageService.GetItemAsStringAsync("TreeBuilder_GroupField"), new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.All,
                    NullValueHandling = NullValueHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.All
                });
                Items = new List<BaseItem>();
                Items = _if.Items;
                Title = _if.Title;
                Uid = _if.Uid;
            }
        }
        
        public Group AddGroup(){
            Group grp = new Group();
            grp.Parent = this;
            Items.Add(grp as Group);
            //Redraw();
            return grp;
        }

        public Interface AddInterface(){
            Interface iface = new Interface();
            iface.Parent = this;
            Items.Add(iface as Interface);
            //Redraw();
            return iface;
        }

        public async void SaveSession()
        {
            await LocalStorageService.SetItemAsync("TreeBuilder_GroupField", this);
        }

    }
}