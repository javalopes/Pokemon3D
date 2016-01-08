using Pokemon3D.DataModel.Json;
using Pokemon3D.Editor.Core.Framework;
using System.Collections.ObjectModel;

namespace Pokemon3D.Editor.Core.Framework
{
    public abstract class DataModelViewModel: ViewModel
    {
        private ObservableCollection<DataModelPropertyViewModel> _properties;
        private string _fileName;

        public ReadOnlyObservableCollection<DataModelPropertyViewModel> Properties { get; }

        public object Model { get; protected set; }

        protected DataModelViewModel(object dataModel, string fileName)
        {
            Model = dataModel;
            FileName = fileName;
            _properties = new ObservableCollection<DataModelPropertyViewModel>();
            Properties = new ReadOnlyObservableCollection<DataModelPropertyViewModel>(_properties);
        }

        internal void AddProperty(DataModelPropertyViewModel property)
        {
            _properties.Add(property);
        }

        public string FileName
        {
            get { return _fileName; }
            set { SetProperty(ref _fileName, value); }
        }

        protected void SetDataViewModelProperty<TModel>(ref TModel model, DataModelViewModel dataModelViewModel)
        {
            model = (TModel)dataModelViewModel.Model;
            Model = dataModelViewModel.Model;
        }
    }
}
