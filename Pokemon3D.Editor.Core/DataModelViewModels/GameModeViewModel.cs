using Pokemon3D.DataModel.Json.GameMode;
using Pokemon3D.Editor.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.Editor.Core.DataModelViewModels
{
    public class GameModeViewModel : DataModelViewModel
    {
        private GameModeModel _model;

        public GameModeViewModel(GameModeModel model)
        {
            _model = model;
            AddProperty(new StringDataModelPropertyViewModel(this, v => _model.Author = v));
            AddProperty(new StringDataModelPropertyViewModel(this, v => _model.Name = v));
            AddProperty(new StringDataModelPropertyViewModel(this, v => _model.Description = v));
            AddProperty(new StringDataModelPropertyViewModel(this, v => _model.Version = v));
        }
    }
}
