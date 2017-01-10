﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering.Compositor;
using System;

namespace Pokemon3D.Rendering.Data
{
    /// <summary>
    /// Holding Geometry Data uploaded to the GPU.
    /// </summary>
    public class Mesh : IDisposable
    {
        public static int InstanceCount;

        private readonly PrimitiveType _primitiveType;
        private VertexBuffer _vertexBuffer;
        private IndexBuffer _indexBuffer;
        private readonly int _primitiveCount;

        public int VertexCount { get; }
        public int IndexCount { get; }
        public GeometryData GeometryData { get; private set; }
        public BoundingBox LocalBounds { get; private set; }
        public bool PreventDrawCallCount { get; set; }

        public Mesh(GraphicsDevice device, GeometryData data, PrimitiveType primitiveType = PrimitiveType.TriangleList, bool holdGeometryData = true)
        {
            _primitiveType = primitiveType;
            GeometryData = holdGeometryData ? data : null;
            VertexCount = data.Vertices.Length;
            IndexCount = data.Indices.Length;

            _vertexBuffer = new VertexBuffer(device, VertexPositionNormalTexture.VertexDeclaration, VertexCount, BufferUsage.WriteOnly);
            _indexBuffer = new IndexBuffer(device, IndexElementSize.SixteenBits, IndexCount, BufferUsage.WriteOnly);

            _vertexBuffer.SetData(data.Vertices);
            _indexBuffer.SetData(data.Indices);

            var min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var max  = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            foreach (var vertex in data.Vertices)
            {
                min.X = MathHelper.Min(min.X, vertex.Position.X);
                min.Y = MathHelper.Min(min.Y, vertex.Position.Y);
                min.Z = MathHelper.Min(min.Z, vertex.Position.Z);

                max.X = MathHelper.Max(max.X, vertex.Position.X);
                max.Y = MathHelper.Max(max.Y, vertex.Position.Y);
                max.Z = MathHelper.Max(max.Z, vertex.Position.Z);
            }
            LocalBounds = new BoundingBox(min, max);

            switch (primitiveType)
            {
                case PrimitiveType.TriangleList:
                    _primitiveCount = IndexCount/3;
                    break;
                case PrimitiveType.TriangleStrip:
                    break;
                case PrimitiveType.LineList:
                    _primitiveCount = IndexCount/2;
                    break;
                case PrimitiveType.LineStrip:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(primitiveType), primitiveType, null);
            }

            InstanceCount++;
        }

        public void Draw()
        {
            var device = _vertexBuffer.GraphicsDevice;
            device.SetVertexBuffer(_vertexBuffer);
            device.Indices = _indexBuffer;

            device.DrawIndexedPrimitives(_primitiveType, 0, 0, _primitiveCount);
            if (!PreventDrawCallCount) RenderStatistics.Instance.DrawCalls++;
        }

        internal Mesh Clone()
        {
            var geometryData = new GeometryData
            {
                Vertices = new VertexPositionNormalTexture[VertexCount], Indices = new ushort[IndexCount],
            };

            _vertexBuffer.GetData(geometryData.Vertices);
            _indexBuffer.GetData(geometryData.Indices);

            return new Mesh(_vertexBuffer.GraphicsDevice, geometryData);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        ~Mesh()
        {
            Dispose(false);
        }
        
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                GC.SuppressFinalize(this);
            }

            _vertexBuffer?.Dispose();
            _vertexBuffer = null;
            _indexBuffer?.Dispose();
            _indexBuffer = null;
            GeometryData = null;
            InstanceCount--;
        }
    }
}
