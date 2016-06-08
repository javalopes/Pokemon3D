using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.Data
{
    /// <summary>
    /// Holding geometry Data for Meshs in the RAM. Can be used for creating
    /// Mesh data independent from the source (file, generated from code, merged...).
    /// </summary>
    public class GeometryData
    {
        /// <summary>
        /// Vertex Data to manipulate or Upload to Mesh.
        /// </summary>
        public VertexPositionNormalTexture[] Vertices;

        /// <summary>
        /// Indices to manipulate or Upload to Mesh.
        /// </summary>
        public ushort[] Indices;

        /// <summary>
        /// Merges a list of GeometryData together.
        /// </summary>
        /// <param name="merges">Merged data</param>
        /// <returns>Merged geometry</returns>
        public static GeometryData Merge(IEnumerable<GeometryDataMerge> merges)
        {
            var vertices = new List<VertexPositionNormalTexture>();
            var indices = new List<ushort>();

            var baseIndex = 0;
            foreach (var geometryDataMerge in merges)
            {
                vertices.AddRange(geometryDataMerge.Data.Vertices.Select(v =>
                {
                    v.Position = Vector3.Transform(v.Position, geometryDataMerge.Transformation);
                    v.TextureCoordinate = v.TextureCoordinate*geometryDataMerge.TextureScale +
                                          geometryDataMerge.TextureOffset;
                    return v;
                }));
                indices.AddRange(geometryDataMerge.Data.Indices.Select(i => (ushort)(i + baseIndex)));
                baseIndex += geometryDataMerge.Data.Vertices.Length;
            }

            return new GeometryData
            {
                Vertices = vertices.ToArray(),
                Indices = indices.ToArray()
            };
        }
    }

    public struct GeometryDataMerge
    {
        public GeometryData Data;

        public Vector2 TextureOffset;

        public Vector2 TextureScale;

        public Matrix Transformation;
    }
}