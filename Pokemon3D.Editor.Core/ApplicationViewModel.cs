using Pokemon3D.DataModel.Json.GameMode;
using Pokemon3D.Editor.Core.DataModelViewModels;
using Pokemon3D.Editor.Core.DetailViewModels;
using Pokemon3D.Editor.Core.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pokemon3D.DataModel.Json.GameMode.Battle;

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
            
            Root = new TreeElementViewModel(this, "Root", TreeElementType.Folder);

            LoadRootFolder(selectedPath);
            LoadContentFolder(selectedPath);
            LoadDataFolder(selectedPath);
            LoadFragmentsFolder(selectedPath);
            LoadMapsFolder(selectedPath);
            LoadScriptsFolder(selectedPath);
            
            Root.SortChildren();
        }

        internal void ShowDetails(ViewModel detailsViewModel)
        {
            ActiveDetails = detailsViewModel;
        }

        private void LoadRootFolder(string selectedPath)
        {
            foreach (var file in GetFilesOfDirectory(selectedPath))
            {
                var fileName = Path.GetFileName(file) ?? "";
                ViewModel details = null;
                if (fileName.Equals("GameMode.json", StringComparison.OrdinalIgnoreCase))
                {
                    details = new GameModeDataViewModel(GameModeModel.FromFile(file));
                }
                Root.AddChild(new TreeElementViewModel(this, fileName) { DetailsViewModel = details });
            }
        }

        private void LoadContentFolder(string selectedPath)
        {
            var contentElement = Root.AddChild(new TreeElementViewModel(this, "Content", TreeElementType.Folder));
            var texturesElement = contentElement.AddChild(new TreeElementViewModel(this, "Textures", TreeElementType.Folder));

            foreach (var file in GetFilesOfDirectory(Path.Combine(selectedPath, "Content", "Textures")))
            {
                texturesElement.AddChild(new TreeElementViewModel(this, Path.GetFileName(file))
                {
                    DetailsViewModel = new TextureDetailViewModel(file)
                });
            }
        }

        private void LoadDataFolder(string selectedPath)
        {
            var dataElement = Root.AddChild(new TreeElementViewModel(this, "Data", TreeElementType.Folder));
            var movesElement = dataElement.AddChild(new TreeElementViewModel(this, "Moves", TreeElementType.Folder));

            foreach (var moveFilePath in Directory.GetFiles(Path.Combine(selectedPath, "Data", "Moves")))
            {
                var fileName = Path.GetFileName(moveFilePath);
                movesElement.AddChild(new TreeElementViewModel(this, fileName, TreeElementType.JsonFile)
                {
                    DetailsViewModel = new MoveDataViewModel(MoveModel.FromFile(moveFilePath), fileName)
                });
            }

            dataElement.AddChild(new TreeElementViewModel(this, "Pokemon", TreeElementType.Folder));
        }

        private void LoadFragmentsFolder(string selectedPath)
        {
            Root.AddChild(new TreeElementViewModel(this, "Fragments", TreeElementType.Folder));
        }

        private void LoadMapsFolder(string selectedPath)
        {
            var mapsElement = Root.AddChild(new TreeElementViewModel(this, "Maps", TreeElementType.Folder));

            var mapPath = Path.Combine(selectedPath, "Maps");
            foreach (var file in GetFilesOfDirectory(mapPath))
            {
                mapsElement.AddChild(new TreeElementViewModel(this, Path.GetFileName(file)));
            }
        }

        private void LoadScriptsFolder(string selectedPath)
        {
            Root.AddChild(new TreeElementViewModel(this, "Scripts", TreeElementType.Folder));
        }
    }
}
