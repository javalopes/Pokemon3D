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
            AddProperty(new StringDataModelPropertyViewModel(s => _model.Id = s, _model.Id) { Caption = "Id" });
            AddProperty(new StringDataModelPropertyViewModel(s => _model.Name = s, _model.Name) { Caption = "Name" });
            AddProperty(new StringDataModelPropertyViewModel(s => _model.Description = s, _model.Description) { Caption = "Description" });
            AddProperty(new EnumDataModelPropertyViewModel(o => _model.MoveCategory = (MoveCategory)o, _model.MoveCategory, typeof(MoveCategory)) {Caption = "MoveCategory"});
            AddProperty(new IntDataModelPropertyViewModel(s => _model.Power = s, _model.Power) { Caption = "Power" });
            AddProperty(new IntDataModelPropertyViewModel(s => _model.Accuracy = s, _model.Accuracy) { Caption = "Accuracy" });
            AddProperty(new IntDataModelPropertyViewModel(s => _model.PP = s, _model.PP) { Caption = "PP" });
            AddProperty(new IntDataModelPropertyViewModel(s => _model.Priority = s, _model.Priority) { Caption = "Priority" });
            AddProperty(new IntDataModelPropertyViewModel(s => _model.TimesToAttack = s, _model.TimesToAttack) { Caption = "TimesToAttack" });
            AddProperty(new EnumDataModelPropertyViewModel(s => _model.Target = (TargetType)s, _model.Target, typeof(TargetType)) { Caption = "Target" });
            AddProperty(new StringDataModelPropertyViewModel(s => _model.ScriptBinding = s, _model.ScriptBinding) { Caption = "ScriptBinding" });
            AddProperty(new BoolDataModelPropertyViewModel(b => _model.IsHMMove = b, _model.IsHMMove) { Caption = "IsHMMove" });
        }
    }
}
