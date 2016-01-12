using Pokemon3D.DataModel.GameMode;
using Pokemon3D.Editor.Core.Framework;

namespace Pokemon3D.Editor.Core.DataModelViewModels
{
    public class GameModeStartConfigrationDataViewModel : DataModelViewModel
    {
        private readonly GameModeStartConfigurationModel _model;

        public GameModeStartConfigrationDataViewModel(GameModeStartConfigurationModel model) : base(model, nameof(GameModeStartConfigurationModel))
        {
            _model = model;
            AddProperty(new StringDataModelPropertyViewModel(v => _model.Map = v, _model.Map, nameof(_model.Map) ));
            AddProperty(new StringDataModelPropertyViewModel(v => _model.Script = v, _model.Script, nameof(_model.Script) ));
        }
    }
}
