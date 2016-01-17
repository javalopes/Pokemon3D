using System;
using Pokemon3D.DataModel;

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

    public class Vector3DataModelPropertyViewModel: SingleValuedDataModelPropertyViewModel<Vector3Model>
    {
        public Vector3DataModelPropertyViewModel(Vector3Model value, string caption) : 
            base(u => { }, value, caption)
        {

        }

        public float X
        {
            get { return Value.X; }
            set { SetProperty(ref Value.X, value); }
        }

        public float Y
        {
            get { return Value.Y; }
            set { SetProperty(ref Value.Y, value); }
        }

        public float Z
        {
            get { return Value.Z; }
            set { SetProperty(ref Value.Z, value); }
        }
    }
}