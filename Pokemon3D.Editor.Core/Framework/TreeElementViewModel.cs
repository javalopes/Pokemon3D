using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Pokemon3D.Editor.Core.Framework
{
    public enum TreeElementType
    {
        Folder,
        JsonFile,
        TextureFile,
        File,
        Model,
    }

    public class TreeElementViewModel : ViewModel
    {
        private ObservableCollection<TreeElementViewModel> _children;
        private string _name;
        private ViewModel _detailsViewModel;
        private ApplicationViewModel _application;

        public ReadOnlyObservableCollection<TreeElementViewModel> Children { get; }
        public CommandViewModel DefaultActionCommand { get; }

        public TreeElementViewModel(ApplicationViewModel application, string name, TreeElementType? treeElementType = null)
        {
            _application = application;
            Name = name;
            Type = treeElementType.GetValueOrDefault(GuessElementTypeByFileExtension(name));
            _children = new ObservableCollection<TreeElementViewModel>();
            Children = new ReadOnlyObservableCollection<TreeElementViewModel>(_children);

            DefaultActionCommand = new CommandViewModel(OnDefaultActionCommand);
        }

        private void OnDefaultActionCommand()
        {
            if (DetailsViewModel != null)
            {
                _application.ShowDetails(DetailsViewModel);
            }
        }

        private TreeElementType GuessElementTypeByFileExtension(string name)
        {
            if (name.EndsWith("json", System.StringComparison.OrdinalIgnoreCase)) return TreeElementType.JsonFile;
            if (name.EndsWith("png", System.StringComparison.OrdinalIgnoreCase)) return TreeElementType.TextureFile;
            if (name.EndsWith("bmp", System.StringComparison.OrdinalIgnoreCase)) return TreeElementType.TextureFile;
            if (name.EndsWith("jpg", System.StringComparison.OrdinalIgnoreCase)) return TreeElementType.TextureFile;
            if (name.EndsWith("jpeg", System.StringComparison.OrdinalIgnoreCase)) return TreeElementType.TextureFile;
            return TreeElementType.File;
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
            set { SetProperty(ref _detailsViewModel, value); }
        }

        public TreeElementType Type { get; private set; }

        public bool IsFolder { get { return Type == TreeElementType.Folder; } }

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
