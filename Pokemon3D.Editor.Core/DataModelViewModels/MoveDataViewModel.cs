using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.DataModel.Json.GameMode.Battle;
using Pokemon3D.Editor.Core.Framework;

namespace Pokemon3D.Editor.Core.DataModelViewModels
{
    //public string Id;
    //public string Name;
    //public string Description;
    //public string[] Types;
    //public int Power;
    //public int Accuracy;
    //public int PP;
    //public int Priority;
    //public int CriticalChance;
    //public int TimesToAttack;
    //public bool IsHMMove;
    //public string ScriptBinding;
    //public string[] Tags;

    public class MoveDataViewModel : DataModelViewModel
    {
        private readonly MoveModel _model;

        public MoveDataViewModel(MoveModel model, string fileName) : base(model, Path.GetFileName(fileName))
        {
            _model = model;
            AddProperty(new StringDataModelPropertyViewModel(s => _model.Id = s, _model.Id) { Caption = "Id" });
            AddProperty(new StringDataModelPropertyViewModel(s => _model.Name = s, _model.Name) { Caption = "Name" });
            AddProperty(new StringDataModelPropertyViewModel(s => _model.Description = s, _model.Description) { Caption = "Description" });
            AddProperty(new IntDataModelPropertyViewModel(s => _model.Power = s, _model.Power) { Caption = "Power" });
            AddProperty(new IntDataModelPropertyViewModel(s => _model.Accuracy = s, _model.Accuracy) { Caption = "Accuracy" });
            AddProperty(new IntDataModelPropertyViewModel(s => _model.PP = s, _model.PP) { Caption = "PP" });
            AddProperty(new IntDataModelPropertyViewModel(s => _model.Priority = s, _model.Priority) { Caption = "Priority" });
            AddProperty(new IntDataModelPropertyViewModel(s => _model.TimesToAttack = s, _model.TimesToAttack) { Caption = "TimesToAttack" });
        }
    }
}
