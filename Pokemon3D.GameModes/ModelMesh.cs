using Assimp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering.Data;
using System.Collections.Generic;
using System.IO;
using System.Windows.Threading;

namespace Pokemon3D.GameModes
{
    public class ModelMesh
    {
        public Rendering.Data.Mesh Mesh { get; private set; }
        public Rendering.Data.Material Material { get; private set; }

        public static ModelMesh[] LoadFromFile(Dispatcher mainThreadDispatcher, GameMode gameMode, string filePath)
        {
            var modelDirectory = Path.GetDirectoryName(filePath);

            AssimpContext context = new AssimpContext();
            var flags = PostProcessSteps.GenerateNormals | PostProcessSteps.GenerateUVCoords | PostProcessSteps.Triangulate;
            var scene = context.ImportFile(filePath, flags);

            var meshs = new List<ModelMesh>();
            foreach (var assimpMesh in scene.Meshes)
            {
                var modelMesh = new ModelMesh(mainThreadDispatcher, gameMode, scene, assimpMesh, modelDirectory);
                meshs.Add(modelMesh);
            }

            return meshs.ToArray();
        }

        public ModelMesh(Dispatcher mainThreadDispatcher, GameMode gameMode, Scene assimpScene, Assimp.Mesh assimpMesh, string modelDirectory)
        {
            var geometryData = GenerateGeometryDataFromAssimpMesh(assimpMesh);

            if (mainThreadDispatcher != null)
            {
                mainThreadDispatcher.Invoke(() => Mesh = new Rendering.Data.Mesh(gameMode.GameContext.GraphicsDevice, geometryData));
            }
            else
            {
                Mesh = new Rendering.Data.Mesh(gameMode.GameContext.GraphicsDevice, geometryData);
            }
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

        private Rendering.Data.Material GenerateMaterialFromMesh(int materialIndex, GameMode gameMode, Scene assimpScene, string modelDirectory)
        {
            var assimpMaterial = assimpScene.Materials[materialIndex];

            Texture2D texture = null;
            if (assimpMaterial.HasTextureDiffuse)
            {
                texture = GetTextureFromSlot(gameMode, modelDirectory, assimpMaterial.TextureDiffuse);
            }

            return new Rendering.Data.Material
            {
                DiffuseTexture = texture,
            };
        }

        private Texture2D GetTextureFromSlot(GameMode gameMode, string modelDirectory, TextureSlot textureSlot)
        {
            if (string.IsNullOrEmpty(textureSlot.FilePath)) return null;

            var fileName = Path.GetFileName(textureSlot.FilePath) ?? "";
            var textureFilePath = Path.Combine(modelDirectory, fileName);

            return gameMode.GetTextureFromRawFolder(textureFilePath);
        }
    }
}