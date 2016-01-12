using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.DataModel.GameMode.Definitions;
using Pokemon3D.FileSystem.Requests;
using Pokemon3D.Rendering.Data;
using System;
using System.Linq;

namespace Pokemon3D.GameModes.Resources
{
    class PrimitiveManager : InstantDataRequestModelManager<PrimitiveModel>
    {
        public PrimitiveManager(GameMode gameMode) : base(gameMode, gameMode.PrimitivesFilePath)
        { }
        
        public GeometryData GetPrimitiveData(string id)
        {
            PrimitiveModel primitiveModel = _modelBuffer.SingleOrDefault(x => x.Id == id);
            if (primitiveModel != null)
            {
                return new GeometryData
                {
                    Vertices = primitiveModel.Vertices.Select(v => new VertexPositionNormalTexture
                    {
                        Position = v.Position.GetVector3(),
                        TextureCoordinate = v.TexCoord.GetVector2(),
                        Normal = v.Normal.GetVector3()
                    }).ToArray(),
                    Indices = primitiveModel.Indices.Select(i => (ushort)i).ToArray()
                };
            }

            throw new ApplicationException("Invalid Primitive Type: " + id);
        }
    }
}
