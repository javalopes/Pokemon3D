using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    static class MeshConverter
    {
        public static byte[] ConvertUnityModelToPmesh(Mesh mesh, Material material)
        {
            if (mesh.normals.Length == 0) mesh.RecalculateNormals();

            using (var memoryStream = new MemoryStream())
            {
                using (var binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write((UInt32)mesh.vertexCount);
                    binaryWriter.Write((UInt32)mesh.triangles.Length);

                    var textureReference = string.Empty;
                    if (material.mainTexture != null)
                    {
                        textureReference = Path.GetFileName(AssetDatabase.GetAssetPath(material.mainTexture)) ?? "";
                    }
                    binaryWriter.Write(textureReference);

                    for (var i = 0; i < mesh.vertexCount; i++)
                    {
                        binaryWriter.Write(mesh.vertices[i].x);
                        binaryWriter.Write(mesh.vertices[i].y);
                        binaryWriter.Write(-mesh.vertices[i].z);
                        binaryWriter.Write(mesh.normals[i].x);
                        binaryWriter.Write(mesh.normals[i].y);
                        binaryWriter.Write(-mesh.normals[i].z);

                        if (mesh.uv.Length == 0)
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

                return memoryStream.GetBuffer();
            }
        }
    }
}
