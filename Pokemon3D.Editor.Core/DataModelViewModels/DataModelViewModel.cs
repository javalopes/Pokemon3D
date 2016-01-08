using System.Collections.ObjectModel;

namespace Pokemon3D.Editor.Core.DataModelViewModels
{
    public abstract class DataModelViewModel
    {
        private ObservableCollection<DataModelPropertyViewModel> _properties;

        public ReadOnlyObservableCollection<DataModelPropertyViewModel> Properties { get; }

        protected DataModelViewModel()
        {
            _properties = new ObservableCollection<DataModelViewModels.DataModelPropertyViewModel>();
            Properties = new ReadOnlyObservableCollection<DataModelViewModels.DataModelPropertyViewModel>(_properties);
        }

        internal void AddProperty(DataModelPropertyViewModel property)
        {
            _properties.Add(property);
        }
    }
}
