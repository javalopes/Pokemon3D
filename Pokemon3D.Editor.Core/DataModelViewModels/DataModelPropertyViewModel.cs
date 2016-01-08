using Pokemon3D.Editor.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.Editor.Core.DataModelViewModels
{
    public abstract class DataModelPropertyViewModel : ViewModel
    {
        private string _caption;

        protected DataModelPropertyViewModel(DataModelViewModel parent)
        {
            parent.AddProperty(this);
        }

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

        public SingleValuedDataModelPropertyViewModel(DataModelViewModel parent, TPropertyType value, Action<TPropertyType> updateModelValue) : base(parent)
        {
            if (updateModelValue == null) throw new ArgumentNullException(nameof(updateModelValue));
            _updateModelValue = updateModelValue;
        }
        
        public TPropertyType Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value, o => _updateModelValue(value)); }
        }
    }

    public class StringDataModelPropertyViewModel : SingleValuedDataModelPropertyViewModel<string>
    {
        public StringDataModelPropertyViewModel(DataModelViewModel parent, Action<string> updateModelValue, string value = null) 
            : base(parent, value, updateModelValue)
        {
        }
    }

    public class FloatDataModelPropertyViewModel : SingleValuedDataModelPropertyViewModel<float>
    {
        public FloatDataModelPropertyViewModel(DataModelViewModel parent, Action<float> updateModelValue, float value = 0.0f) 
            : base(parent, value, updateModelValue)
        {
        }
    }
}
