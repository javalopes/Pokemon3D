using Pokemon3D.DataModel.GameMode;
using Pokemon3D.Editor.Core.Framework;

namespace Pokemon3D.Editor.Core.DataModelViewModels
{
    public class GameModeDataViewModel : DataModelViewModel
    {
        private GameModeModel _model;

        public GameModeDataViewModel(GameModeModel model) : base(model, nameof(GameModeModel))
        {
            _model = model;
            AddProperty(new StringDataModelPropertyViewModel(v => _model.Author = v, _model.Author, nameof(_model.Author)));
            AddProperty(new StringDataModelPropertyViewModel(v => _model.Name = v, _model.Name, nameof(_model.Name)));
            AddProperty(new StringDataModelPropertyViewModel(v => _model.Description = v, _model.Description, nameof(_model.Description)));
            AddProperty(new StringDataModelPropertyViewModel(v => _model.Version = v, _model.Version, nameof(_model.Version)));
            AddProperty(new ObjectDataModelPropertyViewModel(d => SetDataViewModelProperty(ref _model.StartConfiguration, d), 
                                                                  new GameModeStartConfigrationDataViewModel(_model.StartConfiguration),nameof(_model.StartConfiguration)));
        }
    }
}
