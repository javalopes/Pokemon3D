using System.Collections.ObjectModel;
using System.Linq;

namespace Pokemon3D.Editor.Core.Framework
{
    public class TreeElementViewModel : ViewModel
    {
        private ObservableCollection<TreeElementViewModel> _children;
        private string _name;
        private ViewModel _detailsViewModel;

        public ReadOnlyObservableCollection<TreeElementViewModel> Children { get; }

        public TreeElementViewModel(string name, bool isFolder = false)
        {
            Name = name;
            IsFolder = isFolder;
            _children = new ObservableCollection<TreeElementViewModel>();
            Children = new ReadOnlyObservableCollection<TreeElementViewModel>(_children);
        }

        public void SortChildren()
        {
            if (Children.Count == 0) return;

            var elementsSorted = _children.OrderByDescending(c => c.IsFolder).ThenBy(c => c.Name).ToArray();
            _children.Clear();
            foreach(var element in elementsSorted)
            {
                _children.Add(element);
                element.SortChildren();
            }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public ViewModel DetailsViewModel
        {
            get { return _detailsViewModel; }
            private set { SetProperty(ref _detailsViewModel, value); }
        }

        public bool IsFolder { get; private set; }

        public TreeElementViewModel AddChild(TreeElementViewModel child)
        {
            _children.Add(child);
            return child;
        }

        public void RemoveChild(TreeElementViewModel child)
        {
            _children.Remove(child);
        }
    }
}
