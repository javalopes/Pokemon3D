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

        public static ModelMesh[] LoadFromMemory(Dispatcher mainThreadDispatcher, GraphicsDevice device, byte[] data)
        {
            AssimpContext context = new AssimpContext();
            using (var memoryStream = new MemoryStream(data))
            {
                var flags = PostProcessSteps.GenerateNormals | PostProcessSteps.GenerateUVCoords | Assimp.PostProcessSteps.Triangulate;
                var scene = context.ImportFileFromStream(memoryStream, flags, string.Empty);

                var meshs = new List<ModelMesh>();
                foreach (var assimpMesh in scene.Meshes)
                {
                    var modelMesh = new ModelMesh(mainThreadDispatcher, device, scene, assimpMesh);
                    meshs.Add(modelMesh);
                }

                return meshs.ToArray();
            }
        }

        public ModelMesh(Dispatcher mainThreadDispatcher, GraphicsDevice device, Scene assimpScene, Assimp.Mesh assimpMesh)
        {
            var geometryData = GenerateGeometryDataFromAssimpMesh(assimpMesh);

            if (mainThreadDispatcher != null)
            {
                mainThreadDispatcher.Invoke(() => Mesh = new Rendering.Data.Mesh(device, geometryData));
            }
            else
            {
                Mesh = new Rendering.Data.Mesh(device, geometryData);
            }
            Material = GenerateMaterialFromMesh(assimpMesh.MaterialIndex, assimpScene);
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

                var normal = mesh.Normals[i];
                geometryData.Vertices[i].Normal = new Vector3(normal.X, normal.Y, normal.Z);

                var texcoord = mesh.TextureCoordinateChannels[0][i];
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

        private Rendering.Data.Material GenerateMaterialFromMesh(int materialIndex, Scene assimpScene)
        {
            var assimpMaterial = assimpScene.Materials[materialIndex];
            return new Rendering.Data.Material
            {
                //TODO: Refactor
                DiffuseTexture = null
                //DiffuseTexture = string.IsNullOrEmpty(assimpMaterial.TextureDiffuse.FilePath)
                //                        ? null : GetTexture2D(assimpMaterial.TextureDiffuse.FilePath)
            };
        }
    }
}