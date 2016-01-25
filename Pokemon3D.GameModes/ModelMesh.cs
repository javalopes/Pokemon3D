using Assimp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering.Data;

namespace Pokemon3D.GameModes
{
    public class ModelMesh
    {
        public Rendering.Data.Mesh Mesh { get; private set; }
        public Rendering.Data.Material Material { get; private set; }

        public ModelMesh(GraphicsDevice device, Scene assimpScene, Assimp.Mesh assimpMesh)
        {
            Mesh = new Rendering.Data.Mesh(device, GenerateGeometryDataFromAssimpMesh(assimpMesh));
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

        private Rendering.Data.Material GenerateMaterialFromMesh(int materialIndex, Assimp.Scene assimpScene)
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