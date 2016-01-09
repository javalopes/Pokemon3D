using System;

namespace Pokemon3D.Editor.Core.Framework
{
    public class IntDataModelPropertyViewModel : SingleValuedDataModelPropertyViewModel<int>
    {
        public IntDataModelPropertyViewModel(Action<int> updateModelValue, int value, string caption) : base( updateModelValue, value, caption)
        {
        }
    }
}