using Pokemon3D.Editor.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.Editor.Core.Framework
{
    public abstract class DataModelPropertyViewModel : ViewModel
    {
        private string _caption;

        public string Caption
        {
            get { return _caption; }
            set { SetProperty(ref _caption, value); }
        }
    }

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

    public class StringDataModelPropertyViewModel : SingleValuedDataModelPropertyViewModel<string>
    {
        public StringDataModelPropertyViewModel(Action<string> updateModelValue, string value = null) 
            : base(value, updateModelValue)
        {
        }
    }

    public class FloatDataModelPropertyViewModel : SingleValuedDataModelPropertyViewModel<float>
    {
        public FloatDataModelPropertyViewModel(Action<float> updateModelValue, float value = 0.0f) 
            : base(value, updateModelValue)
        {
        }
    }

    public class ObjectDataModelPropertyViewModel : SingleValuedDataModelPropertyViewModel<DataModelViewModel>
    {
        public ObjectDataModelPropertyViewModel(Action<DataModelViewModel> updateModelValue, DataModelViewModel value = null)
            : base(value, updateModelValue)
        {
        }
    }
}
