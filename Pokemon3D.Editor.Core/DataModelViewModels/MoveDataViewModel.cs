using System.IO;
using Pokemon3D.DataModel.Json.GameMode.Battle;
using Pokemon3D.Editor.Core.Framework;

namespace Pokemon3D.Editor.Core.DataModelViewModels
{
    public class MoveDataViewModel : DataModelViewModel
    {
        private readonly MoveModel _model;

        public MoveDataViewModel(MoveModel model) : base(model, nameof(MoveModel))
        {
            _model = model;
            AddProperty(new StringDataModelPropertyViewModel(s => _model.Id = s, _model.Id, "Id" ));
            AddProperty(new StringDataModelPropertyViewModel(s => _model.Name = s, _model.Name, "Name" ));
            AddProperty(new StringDataModelPropertyViewModel(s => _model.Description = s, _model.Description, "Description" ));
            AddProperty(EnumDataModelPropertyViewModel.Create(e => _model.MoveCategory = e, _model.MoveCategory, "MoveCategory"));
            AddProperty(new IntDataModelPropertyViewModel(s => _model.Power = s, _model.Power, "Power"));
            AddProperty(new IntDataModelPropertyViewModel(s => _model.Accuracy = s, _model.Accuracy, "Accuracy" ));
            AddProperty(new IntDataModelPropertyViewModel(s => _model.PP = s, _model.PP, "PP" ));
            AddProperty(new IntDataModelPropertyViewModel(s => _model.Priority = s, _model.Priority, "Priority" ));
            AddProperty(new IntDataModelPropertyViewModel(s => _model.TimesToAttack = s, _model.TimesToAttack, "TimesToAttack" ));
            AddProperty(EnumDataModelPropertyViewModel.Create(e => _model.Target = e, _model.Target, "Target"));
            AddProperty(new StringDataModelPropertyViewModel(s => _model.ScriptBinding = s, _model.ScriptBinding, "ScriptBinding"));
            AddProperty(new BoolDataModelPropertyViewModel(b => _model.IsHMMove = b, _model.IsHMMove, "IsHMMove" ));
        }
    }
}
