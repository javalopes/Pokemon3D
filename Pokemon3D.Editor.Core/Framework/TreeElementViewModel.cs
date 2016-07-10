using Pokemon3D.Editor.Core.DetailViewModels;
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
        private readonly ObservableCollection<TreeElementViewModel> _children;
        private readonly ObservableCollection<CommandViewModel> _commands; 
        private string _name;
        private DetailViewModel _detailsViewModel;
        private readonly ApplicationViewModel _application;

        public ReadOnlyObservableCollection<TreeElementViewModel> Children { get; }
        public ReadOnlyObservableCollection<CommandViewModel> Commands { get; }
        public CommandViewModel DefaultActionCommand { get; }

        public TreeElementViewModel(ApplicationViewModel application, string name, TreeElementType? treeElementType = null)
        {
            _application = application;
            Name = name;
            Type = treeElementType.GetValueOrDefault(GuessElementTypeByFileExtension(name));
            _children = new ObservableCollection<TreeElementViewModel>();
            Children = new ReadOnlyObservableCollection<TreeElementViewModel>(_children);

            _commands = new ObservableCollection<CommandViewModel>();
            Commands = new ReadOnlyObservableCollection<CommandViewModel>(_commands);

            if (treeElementType.GetValueOrDefault(TreeElementType.Folder) != TreeElementType.Folder)
            {
                DefaultActionCommand = new CommandViewModel(OnDefaultActionCommand, "Open");

                _commands.Add(DefaultActionCommand);
            }
        }

        public void AddCommand(CommandViewModel command)
        {
            _commands.Add(command);
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
            if (name.EndsWith("json", StringComparison.OrdinalIgnoreCase)) return TreeElementType.JsonFile;
            if (name.EndsWith("png", StringComparison.OrdinalIgnoreCase)) return TreeElementType.TextureFile;
            if (name.EndsWith("bmp", StringComparison.OrdinalIgnoreCase)) return TreeElementType.TextureFile;
            if (name.EndsWith("jpg", StringComparison.OrdinalIgnoreCase)) return TreeElementType.TextureFile;
            if (name.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase)) return TreeElementType.TextureFile;
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

        public DetailViewModel DetailsViewModel
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
