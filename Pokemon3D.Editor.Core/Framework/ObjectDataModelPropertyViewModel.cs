using System;

namespace Pokemon3D.Editor.Core.Framework
{
    public class ObjectDataModelPropertyViewModel : SingleValuedDataModelPropertyViewModel<DataModelViewModel>
    {
        public ObjectDataModelPropertyViewModel(Action<DataModelViewModel> updateModelValue, DataModelViewModel value = null)
            : base(value, updateModelValue)
        {
        }
    }
}