using Pokemon3D.DataModel.GameMode.Pokemon;
using Pokemon3D.Editor.Core.Framework;

namespace Pokemon3D.Editor.Core.DataModelViewModels
{
    public class PokemonDataViewModel : DataModelViewModel
    {
        private readonly PokemonModel _model;

        public PokemonDataViewModel(PokemonModel model) : base(model, nameof(PokemonModel))
        {
            _model = model;
            AddProperty(new StringDataModelPropertyViewModel(s => _model.Id = s, _model.Id, nameof(_model.Id)));
            AddProperty(new IntDataModelPropertyViewModel(i => _model.Number = i, _model.Number, nameof(_model.Number)));
            AddProperty(new StringDataModelPropertyViewModel(s => _model.Name = s, _model.Name, nameof(_model.Name)));
            AddProperty(new StringDataModelPropertyViewModel(s => _model.InitScript = s, _model.InitScript, nameof(_model.InitScript)));
            AddProperty(EnumDataModelPropertyViewModel.Create(e => _model.ExperienceType = e, _model.ExperienceType, nameof(_model.ExperienceType)));
            AddProperty(new BoolDataModelPropertyViewModel(s => _model.IsLegendary = s, _model.IsLegendary, nameof(_model.IsLegendary)));
            AddProperty(new IntDataModelPropertyViewModel(b => _model.BaseFriendship = b, _model.BaseFriendship, nameof(_model.BaseFriendship)));
            AddProperty(new FloatDataModelPropertyViewModel(b => _model.IsMale = b, (float)_model.IsMale, nameof(_model.IsMale)));
            AddProperty(new BoolDataModelPropertyViewModel(s => _model.IsGenderless = s, _model.IsGenderless, nameof(_model.IsGenderless)));
            AddProperty(new BoolDataModelPropertyViewModel(s => _model.CanBreed = s, _model.CanBreed, nameof(_model.CanBreed)));
            AddProperty(new IntDataModelPropertyViewModel(s => _model.BaseEggSteps = s, _model.BaseEggSteps, nameof(_model.BaseEggSteps)));
            AddProperty(new StringDataModelPropertyViewModel(s => _model.EggPokemon = s, _model.EggPokemon, nameof(_model.EggPokemon)));
            AddProperty(new StringDataModelPropertyViewModel(s => _model.Devolution = s, _model.Devolution, nameof(_model.Devolution)));
        }
    }
}
