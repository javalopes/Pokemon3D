using System;

namespace Pokemon3D.Editor.Core.Framework
{
    public class StringDataModelPropertyViewModel : SingleValuedDataModelPropertyViewModel<string>
    {
        public StringDataModelPropertyViewModel(Action<string> updateModelValue, string value, string caption) 
            : base(updateModelValue, value, caption)
        {
        }
    }
}