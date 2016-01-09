using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.DataModel.Json.GameMode.Map;
using Pokemon3D.Editor.Core.Framework;

namespace Pokemon3D.Editor.Core.DataModelViewModels
{
    public class MapDataViewModel : DataModelViewModel
    {
        private readonly MapModel _model;

        public MapDataViewModel(MapModel dataModel) : base(dataModel, nameof(MapModel))
        {
            _model = dataModel;
            AddProperty(new StringDataModelPropertyViewModel(s => _model.Name = s, _model.Name, "Name"));
            AddProperty(new StringDataModelPropertyViewModel(s => _model.Id = s, _model.Id, "Id"));
            AddProperty(new StringDataModelPropertyViewModel(s => _model.Region = s, _model.Region, "Region"));
            AddProperty(new StringDataModelPropertyViewModel(s => _model.Zone = s, _model.Zone, "Zone"));
            AddProperty(new StringDataModelPropertyViewModel(s => _model.Song = s, _model.Song, "Song"));
            AddProperty(new StringDataModelPropertyViewModel(s => _model.MapScript = s, _model.MapScript, "MapScript"));
            AddProperty(new StringDataModelPropertyViewModel(s => _model.Environment = s, _model.Environment, "Environment"));
        }
    }
}
