using Pokemon3D.DataModel.Json.GameMode;
using Pokemon3D.Editor.Core.Framework;

namespace Pokemon3D.Editor.Core.DataModelViewModels
{
    public class GameModeStartConfigrationDataViewModel : DataModelViewModel
    {
        private GameModeStartConfigurationModel _model;

        public GameModeStartConfigrationDataViewModel(GameModeStartConfigurationModel model) : base(model, "GameModeStartConfiguration.json")
        {
            _model = model;
            AddProperty(new StringDataModelPropertyViewModel(v => _model.Map = v, _model.Map) { Caption = "Map" });
            AddProperty(new StringDataModelPropertyViewModel(v => _model.Script = v, _model.Script) { Caption = "Script" });
        }
    }
}
