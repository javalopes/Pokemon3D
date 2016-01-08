using System;

namespace Pokemon3D.Editor.Core.Framework
{
    public class IntDataModelPropertyViewModel : SingleValuedDataModelPropertyViewModel<int>
    {
        public IntDataModelPropertyViewModel(Action<int> updateModelValue, int value = 0) : base(value, updateModelValue)
        {
        }
    }
}