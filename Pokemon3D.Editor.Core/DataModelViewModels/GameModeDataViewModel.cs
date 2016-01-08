using Pokemon3D.DataModel.Json.GameMode;
using Pokemon3D.Editor.Core.Framework;

namespace Pokemon3D.Editor.Core.DataModelViewModels
{
    public class GameModeDataViewModel : DataModelViewModel
    {
        private GameModeModel _model;

        public GameModeDataViewModel(GameModeModel model) : base("GameMode.json")
        {
            _model = model;
            AddProperty(new StringDataModelPropertyViewModel(v => _model.Author = v, _model.Author) { Caption = "Author" });
            AddProperty(new StringDataModelPropertyViewModel(v => _model.Name = v, _model.Name) { Caption = "Name" });
            AddProperty(new StringDataModelPropertyViewModel(v => _model.Description = v, _model.Description) { Caption = "Description" });
            AddProperty(new StringDataModelPropertyViewModel(v => _model.Version = v, _model.Version) { Caption = "Version" });
        }
    }
}
