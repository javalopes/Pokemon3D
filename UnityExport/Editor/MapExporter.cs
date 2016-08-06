using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Editor
{
    public class MapExporter : EditorWindow
    {
        [MenuItem("Window/Pokemon Map Export")]
        public static void ShowWindow()
        {
            GetWindow(typeof(MapExporter));
        }

        private const string ProgressbarTitle = "Map Export";
        private const string ProgressbarMessage = "Map Export";

        private string _gameModePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private string _mapFileName = "map.json";
        private string _mapId = "mymap";
        private string _mapName = "My Map";
        private ModelExportType _exportModels = ModelExportType.No;
        private bool _createMissingFolders = true;

        public void OnGUI()
        {
            GUILayout.Label("Export Settings", EditorStyles.boldLabel);
            _gameModePath = EditorGUILayout.TextField("path to gamemode", _gameModePath);
            _createMissingFolders = EditorGUILayout.Toggle("create missing folders", _createMissingFolders);
            _exportModels = (ModelExportType)EditorGUILayout.EnumPopup("export 3D models", _exportModels);

            GUILayout.Label("Map Settings", EditorStyles.boldLabel);
            _mapFileName = EditorGUILayout.TextField("file name", _mapFileName);
            _mapId = EditorGUILayout.TextField("map id", _mapId);
            _mapName = EditorGUILayout.TextField("map name", _mapName);

            if (GUILayout.Button("export"))
            {
                ExportScene();
            }
        }

        private void ExportScene()
        {
            EditorUtility.DisplayProgressBar(ProgressbarTitle, ProgressbarMessage, 0);

            try
            {
                var mapExporter = new PokemonMapExporter(_mapFileName, _mapName, _mapId, _gameModePath, _createMissingFolders, _exportModels);

                var allGameObjects = GetSceneGameObjectsFlatten();
                var index = 0;
                foreach (var gameObject in allGameObjects)
                {
                    index++;
                    EditorUtility.DisplayProgressBar(ProgressbarTitle, string.Format("Exporting GameObject {0} of {1}", index, allGameObjects.Count), index / (float)allGameObjects.Count);
                    var relevant = mapExporter.ProcessGameObject(gameObject);

                    if (!relevant) Debug.Log(string.Format("Game Object '{0}' was not exported because no relevant components present.", gameObject.name));
                }

                mapExporter.Finish();

                Debug.Log(string.Format("Exported {0} of {1} game objects.", mapExporter.ExportedEntityCount, mapExporter.TotalEntityCount));
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private static List<GameObject> GetSceneGameObjectsFlatten()
        {
            var list = new List<GameObject>();

            foreach (var gameObject in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                AddGameObjectAndProcessChildren(list, gameObject);
            }

            return list;
        }

        private static void AddGameObjectAndProcessChildren(List<GameObject> data, GameObject gameObject)
        {
            data.Add(gameObject);

            var transform = gameObject.transform;
            for (var i = 0; i <  transform.childCount; i++)
            {
                AddGameObjectAndProcessChildren(data, transform.GetChild(i).gameObject);
            }
        }
    }
}
