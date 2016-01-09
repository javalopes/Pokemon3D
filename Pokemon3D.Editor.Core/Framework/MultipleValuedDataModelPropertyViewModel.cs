using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Pokemon3D.Editor.Core.Framework
{
    public class MultipleValuedDataModelPropertyViewModel<TPropertyType> : DataModelPropertyViewModel
    {
        private readonly Func<TPropertyType> _appendValue;
        private readonly Action<int> _elementRemovedAt;
        private readonly ObservableCollection<TPropertyType> _values;

        public CommandViewModel AddElementCommand { get; private set; }
        public CommandViewModel RemoveElementAtCommand { get; private set; }

        public MultipleValuedDataModelPropertyViewModel(Func<TPropertyType> appendValue, Action<int> elementRemovedAt, IEnumerable<TPropertyType> values, string caption)
        {
            _appendValue = appendValue;
            _elementRemovedAt = elementRemovedAt;

            _values = new ObservableCollection<TPropertyType>(values ?? new TPropertyType[0]);
            Values = new ReadOnlyObservableCollection<TPropertyType>(_values);

            AddElementCommand = new CommandViewModel(OnAddElementCommand);
            RemoveElementAtCommand = new CommandViewModel(OnRemoveElementAtCommand);

            Caption = caption;
        }

        private void OnRemoveElementAtCommand(object parameter)
        {
            if (!(parameter is int)) return;

            var index = (int) parameter;
            if (index < 0 || index >= _values.Count) return;

            _elementRemovedAt(index);
            _values.RemoveAt(index);
        }

        private void OnAddElementCommand()
        {
            _values.Add(_appendValue());
        }

        public ReadOnlyObservableCollection<TPropertyType> Values { get; }
    }
}