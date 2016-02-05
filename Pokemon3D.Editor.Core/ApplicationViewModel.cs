using Pokemon3D.Editor.Core.DetailViewModels;
using Pokemon3D.Editor.Core.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pokemon3D.Editor.Core
{
    public class ApplicationViewModel : ViewModel
    {
        private TreeElementViewModel _root;
        private ViewModel _activeDetails;

        public CommandViewModel OpenGameModeCommand { get; private set; }
        public PlatformService PlatformService { get; private set; }

        public TreeElementViewModel Root
        {
            get { return _root; } 
            set { SetProperty(ref _root, value); }
        }

        public ViewModel ActiveDetails
        {
            get { return _activeDetails; }
            set { SetProperty(ref _activeDetails, value); }
        }

        public ApplicationViewModel(PlatformService platformService)
        {
            if (platformService == null) throw new ArgumentNullException(nameof(platformService));

            PlatformService = platformService;

            OpenGameModeCommand = new CommandViewModel(OnOpenGameModeCommand);
        }

        private IEnumerable<string> GetFilesOfDirectory(string directory)
        {
            if (!Directory.Exists(directory)) return Enumerable.Empty<string>();
            return Directory.GetFiles(directory);
        }

        private void OnOpenGameModeCommand()
        {
            var selectedPath = PlatformService.ShowSelectFolderDialog();
            if (string.IsNullOrEmpty(selectedPath) || !Directory.Exists(selectedPath)) return;

            var gameModeModel = Model.GameModeModel.Open(selectedPath);
            
            Root = new TreeElementViewModel(this, "Root", TreeElementType.Folder);

            var contentElement = Root.AddChild(new TreeElementViewModel(this, "Content", TreeElementType.Folder));
            var texturesElement = contentElement.AddChild(new TreeElementViewModel(this, "Textures", TreeElementType.Folder));
            FillTexturesFolder(texturesElement, gameModeModel);

            var modelsElement = contentElement.AddChild(new TreeElementViewModel(this, "Models", TreeElementType.Folder));

            var dataElement = Root.AddChild(new TreeElementViewModel(this, "Data", TreeElementType.Folder));
            var fragmentsElement = Root.AddChild(new TreeElementViewModel(this, "Fragments", TreeElementType.Folder));
            var MapsElement = Root.AddChild(new TreeElementViewModel(this, "Maps", TreeElementType.Folder));
            var ScriptsElement = Root.AddChild(new TreeElementViewModel(this, "Scripts", TreeElementType.Folder));

            Root.SortChildren();
        }

        private TreeElementViewModel GetElementInHierarchy(TreeElementViewModel root, string[] hierarchyPath)
        {
            var parent = root;
            foreach(var currentPath in hierarchyPath)
            {
                parent = parent.Children.FirstOrDefault(c => c.Name == currentPath) 
                         ?? parent.AddChild(new TreeElementViewModel(this, currentPath, TreeElementType.Folder));
            }
            return parent;
        }

        private void FillTexturesFolder(TreeElementViewModel texturesElement, Model.GameModeModel gameModelModel)
        {
            foreach(var textureModel in gameModelModel.TextureModels.OrderBy(t => t.FilePath))
            {
                var parent = GetElementInHierarchy(texturesElement, textureModel.HierarchyPath);
                parent.AddChild(new TreeElementViewModel(this, textureModel.Name, TreeElementType.TextureFile)
                {
                    DetailsViewModel = new TextureDetailViewModel(textureModel)
                });
            }
        }

        internal void ShowDetails(ViewModel detailsViewModel)
        {
            ActiveDetails = detailsViewModel;
        }
    }
}
