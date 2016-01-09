using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokemon3D.Editor.Core.Framework
{
    public class PrimitiveValueBinder<TPrimitiveType> : ViewModel
    {
        private TPrimitiveType _value;
        private readonly Action<TPrimitiveType, int> _updatePrimitiveValue;

        public PrimitiveValueBinder(Action<TPrimitiveType, int> updatePrimitiveValue, int index)
        {
            _updatePrimitiveValue = updatePrimitiveValue;
            Index = index;
        }

        public static IEnumerable<PrimitiveValueBinder<TPrimitiveType>> BuildList(IEnumerable<TPrimitiveType> values, Action<TPrimitiveType, int> updatePrimitiveValue)
        {
            return values.Select((value,i) => new PrimitiveValueBinder<TPrimitiveType>(updatePrimitiveValue, i)
            {
                Value = value
            });
        }

        public int Index { get; set; }

        public TPrimitiveType Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value, o => _updatePrimitiveValue(value, Index)); }
        }
    }

    public class StringListDataModelPropertyViewModel : MultipleValuedDataModelPropertyViewModel<PrimitiveValueBinder<string>>
    {
        public StringListDataModelPropertyViewModel(Func<PrimitiveValueBinder<string>> appendValue, 
                                                    Action<int> elementRemovedAt,
                                                    Action<string, int> updateValue,
                                                    IEnumerable<string> values, 
                                                    string caption) 
            : base(appendValue, elementRemovedAt, PrimitiveValueBinder<string>.BuildList(values, updateValue), caption)
        {
        }
    }
}