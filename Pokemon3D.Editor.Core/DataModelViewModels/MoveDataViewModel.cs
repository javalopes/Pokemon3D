using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pokemon3D.DataModel.GameMode.Battle;
using Pokemon3D.Editor.Core.Framework;

namespace Pokemon3D.Editor.Core.DataModelViewModels
{
    public class MoveDataViewModel : DataModelViewModel
    {
        private readonly MoveModel _model;

        public MoveDataViewModel(MoveModel model) : base(model, nameof(MoveModel))
        {
            _model = model;
            AddProperty(new StringDataModelPropertyViewModel(s => _model.Id = s, _model.Id, nameof(_model.Id) ));
            AddProperty(new StringDataModelPropertyViewModel(s => _model.Name = s, _model.Name, nameof(_model.Name)));
            AddProperty(new StringListDataModelPropertyViewModel(OnAddType, i => { }, UpdateTypeModelElement, _model.Types, nameof(_model.Types)));
            AddProperty(new StringDataModelPropertyViewModel(s => _model.Description = s, _model.Description, nameof(_model.Description)));
            AddProperty(EnumDataModelPropertyViewModel.Create(e => _model.MoveCategory = e, _model.MoveCategory, nameof(_model.MoveCategory)));
            AddProperty(new IntDataModelPropertyViewModel(s => _model.Power = s, _model.Power, nameof(_model.Power)));
            AddProperty(new IntDataModelPropertyViewModel(s => _model.Accuracy = s, _model.Accuracy, nameof(_model.Accuracy)));
            AddProperty(new IntDataModelPropertyViewModel(s => _model.PP = s, _model.PP, nameof(_model.PP)));
            AddProperty(new IntDataModelPropertyViewModel(s => _model.Priority = s, _model.Priority, nameof(_model.Priority)));
            AddProperty(new IntDataModelPropertyViewModel(s => _model.TimesToAttack = s, _model.TimesToAttack, nameof(_model.TimesToAttack)));
            AddProperty(EnumDataModelPropertyViewModel.Create(e => _model.Target = e, _model.Target, nameof(_model.Target)));
            AddProperty(new StringDataModelPropertyViewModel(s => _model.ScriptBinding = s, _model.ScriptBinding, nameof(_model.ScriptBinding)));
            AddProperty(new BoolDataModelPropertyViewModel(b => _model.IsHMMove = b, _model.IsHMMove, nameof(_model.IsHMMove)));
            AddProperty(new StringListDataModelPropertyViewModel(OnAddTag, i => { }, UpdateTagsModelElement, _model.Tags, nameof(_model.Tags)));
        }

        private void UpdateTagsModelElement(string tag, int index)
        {
            _model.Tags[index] = tag;
        }

        private void UpdateTypeModelElement(string type, int index)
        {
            _model.Types[index] = type;
        }

        private PrimitiveValueBinder<string> OnAddTag()
        {
            _model.Types = _model.Tags.Concat(new[] { "" }).ToArray();
            return new PrimitiveValueBinder<string>(UpdateTagsModelElement, _model.Tags.Length - 1);
        }

        private PrimitiveValueBinder<string> OnAddType()
        {
            _model.Types = _model.Types.Concat(new[] {""}).ToArray();
            return new PrimitiveValueBinder<string>(UpdateTypeModelElement, _model.Types.Length - 1);
        }
    }
}
