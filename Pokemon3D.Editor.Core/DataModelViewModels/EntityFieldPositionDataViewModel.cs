using Pokemon3D.DataModel.GameMode.Map.Entities;
using Pokemon3D.Editor.Core.Framework;

namespace Pokemon3D.Editor.Core.DataModelViewModels
{
    public class EntityFieldPositionDataViewModel : DataModelViewModel
    {
        private EntityFieldPositionModel _model;

        public EntityFieldPositionDataViewModel(EntityFieldPositionModel model) : base(model, nameof(EntityFieldPositionModel))
        {
            _model = model;
            AddProperty(new Vector3DataModelPropertyViewModel(model.Position ?? (model.Position = new DataModel.Vector3Model()), nameof(_model.Position)));
            AddProperty(new Vector3DataModelPropertyViewModel(model.Size ?? (model.Position = new DataModel.Vector3Model {X=1,Y=1,Z=1 }), nameof(_model.Size)));
            AddProperty(new BoolDataModelPropertyViewModel(m => _model.Fill = m, _model.Fill, nameof(_model.Fill)));
            AddProperty(new Vector3DataModelPropertyViewModel(model.Steps ?? (model.Steps = new DataModel.Vector3Model()), nameof(_model.Steps)));
            AddProperty(new Vector3DataModelPropertyViewModel(model.Rotation ?? (model.Rotation = new DataModel.Vector3Model()), nameof(_model.Rotation)));
            AddProperty(new BoolDataModelPropertyViewModel(m => _model.CardinalRotation = m, _model.CardinalRotation, nameof(_model.CardinalRotation)));
            AddProperty(new Vector3DataModelPropertyViewModel(model.Scale ?? (model.Scale = new DataModel.Vector3Model { X = 1, Y = 1, Z = 1 }), nameof(_model.Scale)));
        }
    }
}
