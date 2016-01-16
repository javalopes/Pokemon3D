using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.DataHandling;
using Pokemon3D.DataModel;
using Pokemon3D.DataModel.GameMode.Definitions;
using Pokemon3D.FileSystem.Requests;
using Pokemon3D.Rendering.Data;
using System;
using System.Linq;

namespace Pokemon3D.GameModes.Resources
{
    class PrimitiveManager : AsyncLoadingComponent
    {
        private PrimitiveModel[] _primitives;
        private GameMode _gameMode;

        public PrimitiveManager(GameMode gameMode)
        {
            _gameMode = gameMode;
        }

        public override void InitialLoadData()
        {
            _gameMode.FileLoader.GetFileAsync(_gameMode.PrimitivesFilePath, OnPrimitiveDataReceived);
        }

        private void OnPrimitiveDataReceived(byte[] data)
        {
            _primitives = DataModel<PrimitiveModel[]>.FromByteArray(data);
            LoadingCompleted = true;
        }

        public GeometryData GetPrimitiveData(string id)
        {
            PrimitiveModel primitiveModel = _primitives.SingleOrDefault(x => x.Id == id);
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
