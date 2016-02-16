using Pokemon3D.Editor.Core.DataModelViewModels;
using Pokemon3D.Editor.Core.DetailViewModels;
using Pokemon3D.Editor.Core.Framework;
using Pokemon3D.Editor.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pokemon3D.Editor.Core
{
    public class ApplicationViewModel : ViewModel
    {
        private static readonly Dictionary<ResourceType, TreeElementType> ResourceTypeToTreeElementType = new Dictionary<ResourceType, TreeElementType>
        {
            {ResourceType.File, TreeElementType.File },
            { ResourceType.JsonFile, TreeElementType.JsonFile },
            { ResourceType.Model, TreeElementType.Model },
            { ResourceType.TextureFile, TreeElementType.TextureFile },
        };

        private TreeElementViewModel _root;
        private DetailViewModel _activeDetails;

        public CommandViewModel OpenGameModeCommand { get; private set; }
        public PlatformService PlatformService { get; private set; }

        public TreeElementViewModel Root
        {
            get { return _root; } 
            set { SetProperty(ref _root, value); }
        }

        public DetailViewModel ActiveDetails
        {
            get { return _activeDetails; }
            set { SetProperty(ref _activeDetails, value, old => 
            {
                old?.OnDeactivate();
                _activeDetails?.OnActivate();
            }); }
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

            var gameModeModel = GameModeModel.Open(selectedPath);
            
            Root = new TreeElementViewModel(this, "Root", TreeElementType.Folder);
            var contentElement = Root.AddChild(new TreeElementViewModel(this, "Content", TreeElementType.Folder));
            var dataElement = Root.AddChild(new TreeElementViewModel(this, "Data", TreeElementType.Folder));
            var fragmentsElement = Root.AddChild(new TreeElementViewModel(this, "Fragments", TreeElementType.Folder));
            var MapsElement = Root.AddChild(new TreeElementViewModel(this, "Maps", TreeElementType.Folder));
            var ScriptsElement = Root.AddChild(new TreeElementViewModel(this, "Scripts", TreeElementType.Folder));

            Root.AddChild(new TreeElementViewModel(this, GameModeModel.GameModeJsonFile, TreeElementType.JsonFile)
            {
                DetailsViewModel = new GameModeDataViewModel(gameModeModel.GameModeDataModel)
            });
            
            var texturesElement = contentElement.AddChild(new TreeElementViewModel(this, "Textures", TreeElementType.Folder));
            FillResourcesInHierarchy(texturesElement, gameModeModel.TextureModels, r => new TextureDetailViewModel(r));
            
            var modelsElement = contentElement.AddChild(new TreeElementViewModel(this, "Models", TreeElementType.Folder));
            FillResourcesInHierarchy(modelsElement, gameModeModel.ModelModels, r => new ModelDetailViewModel(PlatformService, r));
                        
            var movesElement = dataElement.AddChild(new TreeElementViewModel(this, "Moves", TreeElementType.Folder));
            foreach(var move in gameModeModel.MoveModels)
            {
                movesElement.AddChild(new TreeElementViewModel(this, move.Id, TreeElementType.JsonFile)
                {
                    DetailsViewModel = new MoveDataViewModel(move)
                });
            }

            var pokemonElement = dataElement.AddChild(new TreeElementViewModel(this, "Pokemon", TreeElementType.Folder));
            foreach (var pokemon in gameModeModel.PokemonModels)
            {
                pokemonElement.AddChild(new TreeElementViewModel(this, pokemon.Id, TreeElementType.JsonFile)
                {
                    DetailsViewModel = new PokemonDataViewModel(pokemon)
                });
            }

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

        private void FillResourcesInHierarchy<TResourceType>(TreeElementViewModel parentElement, 
                                                             IEnumerable<TResourceType> resources, 
                                                             Func<TResourceType, DetailViewModel> createDetails = null) 
            where TResourceType : ResourceModel
        {
            foreach (var resourceModel in resources.OrderBy(t => t.FilePath))
            {
                var parent = GetElementInHierarchy(parentElement, resourceModel.HierarchyPath);
                parent.AddChild(new TreeElementViewModel(this, resourceModel.Name, ResourceTypeToTreeElementType[resourceModel.ResourceType])
                {
                    DetailsViewModel = createDetails != null ? createDetails(resourceModel) : null
                });
            }
        }

        internal void ShowDetails(DetailViewModel detailsViewModel)
        {
            ActiveDetails = detailsViewModel;
        }
    }
}