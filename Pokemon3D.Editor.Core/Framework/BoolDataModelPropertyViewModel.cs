using System;

namespace Pokemon3D.Editor.Core.Framework
{
    public class BoolDataModelPropertyViewModel : SingleValuedDataModelPropertyViewModel<bool>
    {
        public BoolDataModelPropertyViewModel( Action<bool> updateModelValue, bool value) : base(value, updateModelValue)
        {
        }
    }
}