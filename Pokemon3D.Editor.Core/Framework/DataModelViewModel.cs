using System.Collections.ObjectModel;

namespace Pokemon3D.Editor.Core.Framework
{
    public abstract class DataModelViewModel: ViewModel
    {
        private readonly ObservableCollection<DataModelPropertyViewModel> _properties;
        private string _name;

        public ReadOnlyObservableCollection<DataModelPropertyViewModel> Properties { get; }

        public object Model { get; protected set; }

        protected DataModelViewModel(object dataModel, string name)
        {
            Model = dataModel;
            Name = name;
            _properties = new ObservableCollection<DataModelPropertyViewModel>();
            Properties = new ReadOnlyObservableCollection<DataModelPropertyViewModel>(_properties);
        }

        internal void AddProperty(DataModelPropertyViewModel property)
        {
            _properties.Add(property);
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        protected void SetDataViewModelProperty<TModel>(ref TModel model, DataModelViewModel dataModelViewModel)
        {
            model = (TModel)dataModelViewModel.Model;
            Model = dataModelViewModel.Model;
        }
    }
}
