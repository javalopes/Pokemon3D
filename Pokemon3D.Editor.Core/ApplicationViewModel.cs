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

        public CommandViewModel OpenGameModeCommand { get; private set; }
        public PlatformService PlatformService { get; private set; }

        public TreeElementViewModel Root
        {
            get { return _root; } 
            set { SetProperty(ref _root, value); }
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
            
            Root = new TreeElementViewModel("Root", true);
            foreach (var file in GetFilesOfDirectory(selectedPath))
            {
                Root.AddChild(new TreeElementViewModel(Path.GetFileName(file)));
            }

            var contentElement = Root.AddChild(new TreeElementViewModel("Content", true));
            var texturesElement = contentElement.AddChild(new TreeElementViewModel("Textures", true));

            foreach (var file in GetFilesOfDirectory(Path.Combine(selectedPath, "Content", "Textures")))
            {
                texturesElement.AddChild(new TreeElementViewModel(Path.GetFileName(file)));
            }

            Root.AddChild(new TreeElementViewModel("Data", true));
            Root.AddChild(new TreeElementViewModel("Fragments", true));

            var mapsElement = Root.AddChild(new TreeElementViewModel("Maps", true));

            var mapPath = Path.Combine(selectedPath, "Maps");
            foreach (var file in GetFilesOfDirectory(mapPath))
            {
                mapsElement.AddChild(new TreeElementViewModel(Path.GetFileName(file)));
            }

            Root.AddChild(new TreeElementViewModel("Scripts", true));

            Root.SortChildren();
        }
    }
}
