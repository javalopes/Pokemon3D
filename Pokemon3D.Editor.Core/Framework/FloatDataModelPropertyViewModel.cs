using System;

namespace Pokemon3D.Editor.Core.Framework
{
    public class FloatDataModelPropertyViewModel : SingleValuedDataModelPropertyViewModel<float>
    {
        public FloatDataModelPropertyViewModel(Action<float> updateModelValue, float value = 0.0f) 
            : base(value, updateModelValue)
        {
        }
    }
}