using System;
using System.Collections.Generic;
using System.IO;
using Assimp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering.Data;
using Material = Pokemon3D.Rendering.Data.Material;
using Mesh = Pokemon3D.Rendering.Data.Mesh;
using PrimitiveType = Microsoft.Xna.Framework.Graphics.PrimitiveType;

namespace Pokemon3D.GameModes
{

    /// <summary>
    /// Object Holding a Mesh and Material from a file loaded object.
    /// </summary>
    public class ModelMesh
    {
        public Mesh Mesh { get; private set; }
        public Material Material { get; private set; }

        private ModelMesh()
        {
            
        }

        public static ModelMesh[] LoadFromFile(GameMode gameMode, string filePath)
        {
            if (".pmesh".Equals(Path.GetExtension(filePath), StringComparison.OrdinalIgnoreCase))
            {
                return new[] { LoadPMesh(gameMode, filePath) };
            }
            else
            {
                var modelDirectory = Path.GetDirectoryName(filePath);

                var context = new AssimpContext();
                const PostProcessSteps flags = PostProcessSteps.GenerateNormals | PostProcessSteps.GenerateUVCoords
                                               | PostProcessSteps.FlipWindingOrder
                                               | PostProcessSteps.FlipUVs;
                var scene = context.ImportFile(filePath, flags);

                var meshs = new List<ModelMesh>();
                foreach (var assimpMesh in scene.Meshes)
                {
                    var modelMesh = new ModelMesh(gameMode, scene, assimpMesh, modelDirectory);
                    meshs.Add(modelMesh);
                }

                return meshs.ToArray();
            } 
        }

        private static ModelMesh LoadPMesh(GameMode gameMode, string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                using (var binaryReader = new BinaryReader(fileStream))
                {
                    var vertexCount = binaryReader.ReadInt32();
                    var indicesCount = binaryReader.ReadInt32();
                    var textureName = binaryReader.ReadString();

                    var textureFilePath = Path.Combine(Path.GetDirectoryName(filePath) ?? "", Path.GetFileName(textureName) ?? "");
                    Texture2D texture = null;
                    if (File.Exists(textureFilePath))
                    {
                        texture = gameMode.GetTextureFromRawFolder(textureFilePath);
                    }

                    var geometryData = new GeometryData
                    {
                        Vertices = new VertexPositionNormalTexture[vertexCount],
                        Indices = new ushort[indicesCount]
                    };

                    for (var i = 0; i < vertexCount; i++)
                    {
                        var position = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
                        var normal = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
                        var uv = new Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle());
                        geometryData.Vertices[i] = new VertexPositionNormalTexture(position, normal, uv);
                    }

                    for(var i = 0; i < indicesCount; i++)
                    {
                        geometryData.Indices[i] = (ushort) binaryReader.ReadInt32();
                    }

                    return new ModelMesh
                    {
                        Mesh = new Mesh(gameMode.GraphicsDevice, geometryData, PrimitiveType.TriangleList, false),
                        Material = new Material
                        {
                            DiffuseTexture = texture
                        }
                    };
                }
            }
        }

        public ModelMesh(GameMode gameMode, Assimp.Scene assimpScene, Assimp.Mesh assimpMesh, string modelDirectory)
        {
            var geometryData = GenerateGeometryDataFromAssimpMesh(assimpMesh);

            gameMode.EnsureExecutedInMainThread(() => Mesh = new Mesh(gameMode.GraphicsDevice, geometryData));
            Material = GenerateMaterialFromMesh(assimpMesh.MaterialIndex, gameMode, assimpScene, modelDirectory);
        }

        private static GeometryData GenerateGeometryDataFromAssimpMesh(Assimp.Mesh mesh)
        {
            var geometryData = new GeometryData
            {
                Vertices = new VertexPositionNormalTexture[mesh.VertexCount],
                Indices = new ushort[mesh.FaceCount * 3]
            };

            geometryData.Vertices = new VertexPositionNormalTexture[mesh.VertexCount];

            for (var i = 0; i < mesh.VertexCount; i++)
            {
                var vertex = mesh.Vertices[i];
                geometryData.Vertices[i].Position = new Vector3(vertex.X, vertex.Y, vertex.Z);

                var normal = mesh.HasNormals ? mesh.Normals[i] : new Vector3D();
                geometryData.Vertices[i].Normal = new Vector3(normal.X, normal.Y, normal.Z);

                var texcoord = mesh.HasTextureCoords(0) ? mesh.TextureCoordinateChannels[0][i] : new Vector3D();
                geometryData.Vertices[i].TextureCoordinate = new Vector2(texcoord.X, texcoord.Y);
            }

            for (var i = 0; i < mesh.FaceCount; i++)
            {
                geometryData.Indices[i * 3 + 0] = (ushort)mesh.Faces[i].Indices[0];
                geometryData.Indices[i * 3 + 1] = (ushort)mesh.Faces[i].Indices[1];
                geometryData.Indices[i * 3 + 2] = (ushort)mesh.Faces[i].Indices[2];
            }

            return geometryData;
        }

        private Material GenerateMaterialFromMesh(int materialIndex, GameMode gameMode, Assimp.Scene assimpScene, string modelDirectory)
        {
            var assimpMaterial = assimpScene.Materials[materialIndex];

            Texture2D texture = null;
            if (assimpMaterial.HasTextureDiffuse)
            {
                texture = GetTextureFromSlot(gameMode, modelDirectory, assimpMaterial.TextureDiffuse);
            }

            return new Material { DiffuseTexture = texture };
        }

        private Texture2D GetTextureFromSlot(GameMode gameMode, string modelDirectory, TextureSlot textureSlot)
        {
            if (string.IsNullOrEmpty(textureSlot.FilePath)) return null;

            var fileName = Path.GetFileName(textureSlot.FilePath) ?? "";
            var textureFilePath = Path.Combine(modelDirectory, fileName);

            if (!File.Exists(fileName)) return null;

            return gameMode.GetTextureFromRawFolder(textureFilePath);
        }
    }
}
