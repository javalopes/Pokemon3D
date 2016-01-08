using System;

namespace Pokemon3D.Editor.Core.Framework
{
    public class SingleValuedDataModelPropertyViewModel<TPropertyType> : DataModelPropertyViewModel
    {
        private TPropertyType _value;
        private Action<TPropertyType> _updateModelValue;

        public SingleValuedDataModelPropertyViewModel(TPropertyType value, Action<TPropertyType> updateModelValue)
        {
            if (updateModelValue == null) throw new ArgumentNullException(nameof(updateModelValue));
            _updateModelValue = updateModelValue;
            Value = value;
        }
        
        public TPropertyType Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value, o => _updateModelValue(value)); }
        }
    }
}