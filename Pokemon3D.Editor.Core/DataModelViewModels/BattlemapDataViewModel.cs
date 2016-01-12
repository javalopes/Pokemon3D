using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.DataModel.GameMode.Map;
using Pokemon3D.Editor.Core.Framework;

namespace Pokemon3D.Editor.Core.DataModelViewModels
{
    public class BattlemapDataViewModel : DataModelViewModel
    {
        private BattleMapDataModel _model;

        public BattlemapDataViewModel(BattleMapDataModel dataModel) : base(dataModel, nameof(BattleMapDataModel))
        {
            _model = dataModel;

            AddProperty(new StringDataModelPropertyViewModel(s => _model.BattleMapFile = s, _model.BattleMapFile, nameof(_model.BattleMapFile)));
        }
    }
}
