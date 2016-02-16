using Pokemon3D.Editor.Core.DetailViewModels;
using System.Collections.ObjectModel;
using System;

namespace Pokemon3D.Editor.Core.Framework
{
    public class DataModelViewModel: DetailViewModel
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

        internal override void OnActivate()
        {
            
        }

        internal override void OnDeactivate()
        {
        }
    }
}
