using System;

namespace Pokemon3D.Editor.Core.Framework
{
    public class StringDataModelPropertyViewModel : SingleValuedDataModelPropertyViewModel<string>
    {
        public StringDataModelPropertyViewModel(Action<string> updateModelValue, string value = null) 
            : base(value, updateModelValue)
        {
        }
    }
}