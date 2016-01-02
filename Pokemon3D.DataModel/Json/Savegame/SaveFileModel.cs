using Pokemon3D.DataModel.Json.Savegame.Inventory;
using Pokemon3D.DataModel.Json.Savegame.Pokemon;
using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.Savegame
{
    /// <summary>
    /// Contains the saved information about a game file.
    /// </summary>
    [DataContract]
    class SaveFileModel : DataModel<SaveFileModel>
    {
        [DataMember(Order = 0)]
        public string GameMode;

        [DataMember(Order = 1)]
        public PlayerModel PlayerData;

        [DataMember(Order = 2)]
        public PokemonSaveModel[] Pokemon;

        [DataMember(Order = 3)]
        public InventoryItemModel[] Items;

        [DataMember(Order = 4)]
        public PokedexSaveModel Pokedex;

        public override object Clone()
        {
            var clone = (SaveFileModel)MemberwiseClone();
            clone.PlayerData = PlayerData.CloneModel();
            clone.Pokemon = (PokemonSaveModel[])Pokemon.Clone();
            clone.Items = (InventoryItemModel[])Items.Clone();
            clone.Pokedex = Pokedex.CloneModel();
            return clone;
        }
    }
}
