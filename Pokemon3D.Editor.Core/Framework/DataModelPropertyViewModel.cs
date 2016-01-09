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
}
