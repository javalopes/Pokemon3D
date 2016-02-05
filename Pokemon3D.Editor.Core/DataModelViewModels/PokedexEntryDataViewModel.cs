using Pokemon3D.DataModel.GameMode.Pokemon;
using Pokemon3D.Editor.Core.Framework;

namespace Pokemon3D.Editor.Core.DataModelViewModels
{
    public class PokedexEntryDataViewModel : DataModelViewModel
    {
        private readonly PokedexEntryModel _model;

        public PokedexEntryDataViewModel(PokedexEntryModel model) : base(model, nameof(PokedexEntryModel))
        {
            _model = model;
            AddProperty(new StringDataModelPropertyViewModel(s => _model.Text = s, _model.Text, nameof(_model.Text)));
            AddProperty(new StringDataModelPropertyViewModel(s => _model.Species = s, _model.Species, nameof(_model.Species)));
            AddProperty(new DoubleDataModelPropertyViewModel(s => _model.Height = s, _model.Height, nameof(_model.Height)));
            AddProperty(new DoubleDataModelPropertyViewModel(s => _model.Weight = s, _model.Weight, nameof(_model.Weight)));
            AddProperty(new ColorDataModelPropertyViewModel(_model.Color, nameof(_model.Color)));
            AddProperty(new StringDataModelPropertyViewModel(s => _model.BodyStyle = s, _model.BodyStyle, nameof(_model.BodyStyle)));
        }
    }
}
