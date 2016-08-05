using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Text;
using Assets.Editor.Data;
using System.IO;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

enum ModelExportType
{
    No,
    OriginalModelFile,
    CustomModelFormat
}

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
    private string _mapFilePath = "map.json";
    private string _mapId = "mymap";
    private string _mapName = "My Map";
    private ModelExportType _exportModels = ModelExportType.No;
    private bool _createMissingFolders = true;

    private int _totalEntityCount;
    private int _exportedEntityCount;
    private StringBuilder _stringBuilder = new StringBuilder();
    
    public void OnGUI()
    {
        GUILayout.Label("Export Settings", EditorStyles.boldLabel);
        _gameModePath = EditorGUILayout.TextField("path to gamemode", _gameModePath);
        _createMissingFolders = EditorGUILayout.Toggle("create missing folders", _createMissingFolders);
        _exportModels = (ModelExportType)EditorGUILayout.EnumPopup("export 3D models", _exportModels);

        GUILayout.Label("Map Settings", EditorStyles.boldLabel);
        _mapFilePath = EditorGUILayout.TextField("file name", _mapFilePath);
        _mapId = EditorGUILayout.TextField("map id", _mapId);
        _mapName = EditorGUILayout.TextField("map name", _mapName);

        if (GUILayout.Button("export")) ExportScene(_mapFilePath);
    }

    private void ExportScene(string filePath)
    {
        var basicPathToExport = PreparePathsAndFolders();
        if (!filePath.EndsWith(".json", StringComparison.OrdinalIgnoreCase)) filePath = filePath + ".json";

        EditorUtility.DisplayProgressBar(ProgressbarTitle, ProgressbarMessage, 0);

        try
        {
            _totalEntityCount = 0;
            _exportedEntityCount = 0;
            _stringBuilder = new StringBuilder();

            var scene = SceneManager.GetActiveScene();
            var mapModel = new MapModel
            {
                Name = _mapName,
                Id = _mapId
            };

            var rootObjects = scene.GetRootGameObjects();
            var index = 0;
            foreach(var gameObject in rootObjects)
            {
                ExportGameObject(basicPathToExport, mapModel, gameObject);
                EditorUtility.DisplayProgressBar(ProgressbarTitle, ProgressbarMessage, index++ / (float)rootObjects.Length);
            }

            File.WriteAllText(Path.Combine(GetMapPath(basicPathToExport), filePath), JsonUtility.ToJson(mapModel, true));

            Debug.Log(string.Format("Exported {0} of {1} game objects.", _exportedEntityCount, _totalEntityCount));
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
    }

    private static string GetMapPath(string basePath)
    {
        return Path.Combine(basePath, "Maps");
    }

    private static string GetModelsPath(string basePath)
    {
        return Path.Combine(Path.Combine(basePath, "Content"), "Models");
    }

    private string PreparePathsAndFolders()
    {
        var basicPathToExport = _gameModePath;
        if (string.IsNullOrEmpty(basicPathToExport))
        {
            basicPathToExport = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        if(_createMissingFolders)
        {
            if(!Directory.Exists(basicPathToExport)) Directory.CreateDirectory(basicPathToExport);

            var mapPath = GetMapPath(basicPathToExport);
            if(!Directory.Exists(mapPath)) Directory.CreateDirectory(mapPath);

            var modelsPath = GetModelsPath(basicPathToExport);
            if (!Directory.Exists(modelsPath)) Directory.CreateDirectory(modelsPath);
        }
        
        return basicPathToExport;
    }

    private void ExportGameObject(string basicPathToExport, MapModel model, GameObject gameObject)
    {
        _totalEntityCount++;
        
        var camera = gameObject.GetComponent<Camera>();
        _stringBuilder = new StringBuilder();

        if (camera != null)
        {
            _stringBuilder.Append(string.Format("Game Object '{0}' was not exported because it has a camera attached.",
                gameObject.name));
        }
        else if (HasRelevantComponents(gameObject))
        {
            _stringBuilder.Append(string.Format("Game Object '{0}' was exported", gameObject.name));
            _exportedEntityCount++;

            var entityFieldModel = new EntityFieldModel();
            model.Entities.Add(entityFieldModel);
            entityFieldModel.Placing.Add(CreatePositioningFromTransform(gameObject.transform));

            entityFieldModel.Entity = new EntityModel();
            MayAttachVisualModel(basicPathToExport, entityFieldModel.Entity, gameObject);
        }
        else
        {
            _stringBuilder.Append(
                string.Format("Gameobject '{0}' not processed because no relevant components are attached.",
                    gameObject.name));
        }

        if (_stringBuilder.Length > 0) Debug.Log(_stringBuilder.ToString());

        for (var i = 0; i < gameObject.transform.childCount; i++)
        {
            ExportGameObject(basicPathToExport, model, gameObject.transform.GetChild(i).gameObject);
        }
    }

    private bool HasRelevantComponents(GameObject gameObject)
    {
        return gameObject.GetComponent<MeshFilter>() != null && gameObject.GetComponent<MeshRenderer>() != null;
    }

    private void MayAttachVisualModel(string basicExportPath, EntityModel entity, GameObject gameObject)
    {
        var meshRendererComponent = gameObject.GetComponent<MeshRenderer>();
        var meshFilterComponent = gameObject.GetComponent<MeshFilter>();
        if (meshFilterComponent != null && meshRendererComponent != null)
        {
            var path = AssetDatabase.GetAssetPath(meshFilterComponent.sharedMesh ?? meshFilterComponent.mesh);

            entity.Id = gameObject.name;
            entity.IsStatic = gameObject.isStatic;

            var componentModel = new EntityComponentModel { Id = "VisualModel" };
            var meshReferenceItem = new EntityComponentDataItemModel
            {
                Key = "MeshReference",
                Value = path
            };
            componentModel.Data.Add(meshReferenceItem);

            entity.Components.Add(componentModel);

            var exportsModel = _exportModels != ModelExportType.No;

            if(!string.IsNullOrEmpty(path) && exportsModel)
            {
                if(_exportModels == ModelExportType.OriginalModelFile)
                {
                    meshReferenceItem.Value = ExportAssetTo(path, GetModelsPath(basicExportPath));
                }
                else
                {
                    meshReferenceItem.Value = ExportModelTo(meshFilterComponent.sharedMesh ?? meshFilterComponent.mesh,
                                                            meshRendererComponent.material, 
                                                            path, 
                                                            GetModelsPath(basicExportPath));
                }
            }

            var texture = meshRendererComponent.sharedMaterial.mainTexture;
            var textureAssetPath = AssetDatabase.GetAssetPath(texture);
            if(!string.IsNullOrEmpty(textureAssetPath) && exportsModel)
            {
                ExportAssetTo(textureAssetPath, GetModelsPath(basicExportPath));
            }

            _stringBuilder.Append(" It is a visual model.");
        }
    }

    private static string ExportAssetTo(string assetPath, string targetFolder)
    {
        var targetFilePath = Path.Combine(targetFolder, Path.GetFileName(assetPath) ?? "");
        if (!File.Exists(targetFilePath)) File.Copy(assetPath, targetFilePath);

        return Path.GetFileName(targetFilePath);
    }

    private static string ExportModelTo(Mesh mesh, Material material, string assetPath, string targetFolder)
    {
        var targetFilePath = Path.Combine(targetFolder, (Path.GetFileNameWithoutExtension(assetPath) ?? "") + ".pmesh");

        if(File.Exists(targetFilePath)) return Path.GetFileName(targetFilePath);

        if (mesh.normals.Length == 0) mesh.RecalculateNormals();

        using(var fileStream = new FileStream(targetFilePath, FileMode.Create))
        {
            using(var binaryWriter = new BinaryWriter(fileStream))
            {
                binaryWriter.Write((UInt32)mesh.vertexCount);
                binaryWriter.Write((UInt32)mesh.triangles.Length);

                var textureReference = string.Empty;
                if(material.mainTexture != null)
                {
                    textureReference = Path.GetFileName(AssetDatabase.GetAssetPath(material.mainTexture)) ?? "";
                }
                binaryWriter.Write(textureReference);

                Assert.AreEqual(mesh.vertices.Length, mesh.normals.Length);
                Assert.AreEqual(mesh.normals.Length, mesh.uv.Length);

                for (var i = 0; i < mesh.vertexCount; i++)
                {
                    binaryWriter.Write(mesh.vertices[i].x);
                    binaryWriter.Write(mesh.vertices[i].y);
                    binaryWriter.Write(-mesh.vertices[i].z);
                    binaryWriter.Write(mesh.normals[i].x);
                    binaryWriter.Write(mesh.normals[i].y);
                    binaryWriter.Write(-mesh.normals[i].z);

                    if(mesh.uv.Length == 0)
                    {
                        binaryWriter.Write(0.0f);
                        binaryWriter.Write(0.0f);
                    }
                    else
                    {
                        binaryWriter.Write(mesh.uv[i].x);
                        binaryWriter.Write(1.0f - mesh.uv[i].y);
                    }
                    
                }

                for (var i = 0; i < mesh.triangles.Length; i++) binaryWriter.Write(mesh.triangles[i]);
            }
        }

        return Path.GetFileName(targetFilePath);
    }

    private static EntityFieldPositionModel CreatePositioningFromTransform(Transform transform)
    {
        return new EntityFieldPositionModel
        {
            CardinalRotation = false,
            Fill = false,
            Size = new Vector3Model { X = 1, Y = 1, Z = 1 },
            Steps = new Vector3Model { X = 1, Y = 1, Z = 1 },
            Position =
                new Vector3Model
                {
                    X = transform.position.x,
                    Y = transform.position.y,
                    Z = -transform.position.z
                },
            Rotation =
                new Vector3Model
                {
                    X = -transform.rotation.eulerAngles.x,
                    Y = -transform.rotation.eulerAngles.z,
                    Z = transform.rotation.eulerAngles.y
                },
            Scale =
                new Vector3Model
                {
                    X = transform.lossyScale.x,
                    Y = transform.lossyScale.y,
                    Z = transform.lossyScale.z
                }
        };
    }
}
