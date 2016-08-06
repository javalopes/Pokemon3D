using System;
using System.IO;
using Assets.Editor.Data;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    internal class PokemonMapExporter
    {
        public int ExportedEntityCount { get; private set; }
        public int TotalEntityCount { get; private set; }

        private readonly string _mapFileName;
        private readonly ModelExportType _exportType;
        private readonly FilePathExporter _filePathExporter;

        private readonly MapModel _mapModel;

        public PokemonMapExporter(string mapFileName, string mapName, string mapId, string gameModeFolder, bool createMissingFolders, ModelExportType exportType)
        {
            _mapFileName = mapFileName;
            _exportType = exportType;
            _filePathExporter = new FilePathExporter(gameModeFolder, createMissingFolders);

            _mapModel = new MapModel
            {
                Name = mapName,
                Id = mapId
            };

            ExportedEntityCount = 0;
            TotalEntityCount = 0;
        }

        public bool ProcessGameObject(GameObject gameObject)
        {
            TotalEntityCount++;
            if (HasRelevantComponents(gameObject))
            {
                var entityFieldModel = new EntityFieldModel();
                _mapModel.Entities.Add(entityFieldModel);
                entityFieldModel.Placing.Add(CreatePositioningFromTransform(gameObject.transform));
                entityFieldModel.Entity = new EntityModel();

                MayAttachVisualModel(entityFieldModel.Entity, gameObject);
                ExportedEntityCount++;
                return true;
            }

            return false;
        }

        private void MayAttachVisualModel(EntityModel entity, GameObject gameObject)
        {
            var meshRendererComponent = gameObject.GetComponent<MeshRenderer>();
            var meshFilterComponent = gameObject.GetComponent<MeshFilter>();
            if (meshFilterComponent == null || meshRendererComponent == null) return;

            entity.Id = gameObject.name;
            entity.IsStatic = gameObject.isStatic;

            var componentModel = new EntityComponentModel { Id = "VisualModel" };
            var meshReferenceItem = new EntityComponentDataItemModel
            {
                Key = "MeshReference"
            };
            componentModel.Data.Add(meshReferenceItem);
            entity.Components.Add(componentModel);

            var exportsModel = _exportType != ModelExportType.No;
            var path = AssetDatabase.GetAssetPath(meshFilterComponent.sharedMesh ?? meshFilterComponent.mesh);

            if (!string.IsNullOrEmpty(path) && exportsModel)
            {
                if (_exportType == ModelExportType.OriginalModelFile)
                {
                    meshReferenceItem.Value = _filePathExporter.ExportAssetToModels(path);
                }
                else
                {
                    meshReferenceItem.Value = ConvertModelToPmeshAndSaveToFolder(meshFilterComponent.sharedMesh ?? meshFilterComponent.mesh,
                        meshRendererComponent.material,
                        path);
                }
            }

            var texture = meshRendererComponent.sharedMaterial.mainTexture;
            var textureAssetPath = AssetDatabase.GetAssetPath(texture);
            if (!string.IsNullOrEmpty(textureAssetPath) && exportsModel)
            {
                _filePathExporter.ExportAssetToModels(textureAssetPath);
            }
        }

        private string ConvertModelToPmeshAndSaveToFolder(Mesh mesh, Material material, string assetPath)
        {
            var data = MeshConverter.ConvertUnityModelToPmesh(mesh, material);
            return _filePathExporter.ExportAssetToModels(assetPath, data);
        }

        private static bool HasRelevantComponents(GameObject gameObject)
        {
            return gameObject.GetComponent<MeshFilter>() != null && gameObject.GetComponent<MeshRenderer>() != null;
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

        public void Finish()
        {
            var filePath = _mapFileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase)
                ? _mapFileName
                : _mapFileName + ".json";
            File.WriteAllText(_filePathExporter.GetFolderPathMap(filePath), JsonUtility.ToJson(_mapModel, true));
        }
    }
}
