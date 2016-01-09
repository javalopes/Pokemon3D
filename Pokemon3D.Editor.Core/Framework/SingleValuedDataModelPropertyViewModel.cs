using System;

namespace Pokemon3D.Editor.Core.Framework
{
    public class SingleValuedDataModelPropertyViewModel<TPropertyType> : DataModelPropertyViewModel
    {
        private TPropertyType _value;
        private readonly Action<TPropertyType> _updateModelValue;

        public SingleValuedDataModelPropertyViewModel(Action<TPropertyType> updateModelValue, TPropertyType value, string caption)
        {
            if (updateModelValue == null) throw new ArgumentNullException(nameof(updateModelValue));
            _updateModelValue = updateModelValue;
            Value = value;
            Caption = caption;
        }
        
        public TPropertyType Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value, o => _updateModelValue(value)); }
        }
    }
}