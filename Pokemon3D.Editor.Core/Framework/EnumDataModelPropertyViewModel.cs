using System;

namespace Pokemon3D.Editor.Core.Framework
{
    public class EnumDataModelPropertyViewModel : SingleValuedDataModelPropertyViewModel<object>
    {
        public Type EnumType { get; private set; }

        public EnumDataModelPropertyViewModel(Action<object> updateModelValue, object value, Type enumType) : base(value, updateModelValue)
        {
            EnumType = enumType;
        }
    }
}