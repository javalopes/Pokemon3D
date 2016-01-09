using System;
using System.Security.Policy;

namespace Pokemon3D.Editor.Core.Framework
{
    public class EnumDataModelPropertyViewModel : SingleValuedDataModelPropertyViewModel<object>
    {
        public Type EnumType { get; private set; }

        public EnumDataModelPropertyViewModel(Action<object> updateModelValue, object value, Type enumType, string caption) : base(updateModelValue, value, caption)
        {
            EnumType = enumType;
        }

        public static EnumDataModelPropertyViewModel Create<TEnum>(Action<TEnum> updateModelValue, TEnum value, string caption)
        {
            return new EnumDataModelPropertyViewModel(o => updateModelValue((TEnum)o), value, typeof(TEnum), caption);
        }
    }
}