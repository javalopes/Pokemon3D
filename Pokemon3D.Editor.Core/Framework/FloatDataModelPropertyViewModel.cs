using System;

namespace Pokemon3D.Editor.Core.Framework
{
    public class FloatDataModelPropertyViewModel : SingleValuedDataModelPropertyViewModel<float>
    {
        public FloatDataModelPropertyViewModel(Action<float> updateModelValue, float value, string caption) 
            : base(updateModelValue, value, caption)
        {
        }
    }
}