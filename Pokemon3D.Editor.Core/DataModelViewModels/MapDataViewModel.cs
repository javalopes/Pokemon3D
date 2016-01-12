using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.DataModel.GameMode.Map;
using Pokemon3D.Editor.Core.Framework;

namespace Pokemon3D.Editor.Core.DataModelViewModels
{
    public class MapDataViewModel : DataModelViewModel
    {
        private readonly MapModel _model;

        public MapDataViewModel(MapModel dataModel) : base(dataModel, nameof(MapModel))
        {
            _model = dataModel;
            AddProperty(new StringDataModelPropertyViewModel(s => _model.Name = s, _model.Name, nameof(_model.Name)));
            AddProperty(new StringDataModelPropertyViewModel(s => _model.Id = s, _model.Id, nameof(_model.Id)));
            AddProperty(new StringDataModelPropertyViewModel(s => _model.Region = s, _model.Region, nameof(_model.Region)));
            AddProperty(new StringDataModelPropertyViewModel(s => _model.Zone = s, _model.Zone, nameof(_model.Zone)));
            AddProperty(new StringDataModelPropertyViewModel(s => _model.Song = s, _model.Song, nameof(_model.Song)));
            AddProperty(new StringDataModelPropertyViewModel(s => _model.MapScript = s, _model.MapScript, nameof(_model.MapScript)));
            AddProperty(new StringDataModelPropertyViewModel(s => _model.Environment = s, _model.Environment, nameof(_model.Environment)));
            AddProperty(new ObjectDataModelPropertyViewModel(d => _model.BattleMapData = (BattleMapDataModel)d.Model, new BattlemapDataViewModel(_model.BattleMapData), nameof(_model.BattleMapData)));
        }
    }
}
