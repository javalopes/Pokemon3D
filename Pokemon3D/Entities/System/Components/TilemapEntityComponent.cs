using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;
using Pokemon3D.Common.Extensions;
using Pokemon3D.GameModes;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Data;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.Entities.System.Components
{
    [JsonComponentId("tilemap")]
    internal class TilemapEntityComponent : EntityComponent
    {
        private DrawableElement _drawableElement;

        private TilemapEntityComponent(Entity referringEntity)
            : base(referringEntity)
        {
            
        }

        public TilemapEntityComponent(EntityComponentDataCreationStruct parameters) : base(parameters)
        {
            var gameMode = GameInstance.GetService<GameModeManager>().ActiveGameMode;

            _drawableElement = GameInstance.GetService<SceneRenderer>().CreateDrawableElement(true);

            _drawableElement.Material = new Material
            {
                Color = new Color(GetDataOrDefault("Shading", Vector3.One)),
                CastShadow = true,
                ReceiveShadow = true,
                UseTransparency = false,
                IsUnlit = false,
                DiffuseTexture = gameMode.GetTexture(GetData<string>("Texture"))
            };

            _drawableElement.IsActive = ReferringEntity.IsActive;

            var geometryData = CreateTilemapData();
            GameInstance.GetService<JobSystem>().EnsureExecutedInMainThread(() => _drawableElement.Mesh = new Mesh(GameInstance.GetService<GraphicsDevice>(), geometryData, holdGeometryData: false));

            if (!ReferringEntity.IsInitializing) _drawableElement.EndInitialzing();
        }

        private GeometryData CreateTilemapData()
        {
            var tilesX = GetData<int>("tilesX");
            var tilesY = GetData<int>("tilesY");
            var tileElementSize = GetData<int>("tileElementSize");
            var diffuseTexture = _drawableElement.Material.DiffuseTexture;
            var tilesetElementsX = diffuseTexture.Width / tileElementSize;

            var geometryData = new GeometryData
            {
                Vertices = new VertexPositionNormalTexture[tilesX*tilesY*4],
                Indices = new ushort[tilesX*tilesY*6]
            };

            var tiles = GetData<string>("map")
                        .Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse).ToArray();

            var texturePartSize = diffuseTexture.GetTexcoordsFromPixelCoords(tileElementSize, tileElementSize);

            var vertexBaseIndex = 0;
            var indexBaseIndex = 0;
            for (var i = 0; i < Math.Min(tiles.Length, tilesX*tilesY); i++)
            {
                var element = tiles[i];
                if (element == 0) continue;
                var tilesetX = (element-1)%tilesetElementsX;
                var tilesetY = (element - 1) / tilesetElementsX;

                var mapX = i%tilesX;
                var mapY = i/tilesX;

                var position = new Vector3(mapX, 0.0f, mapY);
                var texcoords = diffuseTexture.GetTexcoordsFromPixelCoords(tilesetX* tileElementSize, tilesetY* tileElementSize);
                var normal = Vector3.UnitY;

                geometryData.Vertices[vertexBaseIndex+0] = new VertexPositionNormalTexture(position + new Vector3(0,0,-1), normal, texcoords + new Vector2(0, texturePartSize.Y));
                geometryData.Vertices[vertexBaseIndex+1] = new VertexPositionNormalTexture(position + new Vector3(0,0, 0), normal, texcoords + new Vector2(0,0));
                geometryData.Vertices[vertexBaseIndex+2] = new VertexPositionNormalTexture(position + new Vector3(1,0, -1), normal, texcoords + new Vector2(texturePartSize.X, texturePartSize.Y));
                geometryData.Vertices[vertexBaseIndex+3] = new VertexPositionNormalTexture(position + new Vector3(1, 0, 0), normal, texcoords + new Vector2(texturePartSize.X, 0));

                geometryData.Indices[indexBaseIndex + 0] = (ushort) vertexBaseIndex;
                geometryData.Indices[indexBaseIndex + 1] = (ushort) (vertexBaseIndex+2);
                geometryData.Indices[indexBaseIndex + 2] = (ushort) (vertexBaseIndex+1);
                geometryData.Indices[indexBaseIndex + 3] = (ushort) (vertexBaseIndex+1);
                geometryData.Indices[indexBaseIndex + 4] = (ushort) (vertexBaseIndex+2);
                geometryData.Indices[indexBaseIndex + 5] = (ushort) (vertexBaseIndex+3);

                vertexBaseIndex += 4;
                indexBaseIndex += 6;
            }

            return geometryData;
        }

        public override void OnIsActiveChanged()
        {
            _drawableElement.IsActive = IsActive;
        }

        public override void OnInitialized()
        {
            _drawableElement.EndInitialzing();
        }

        public override EntityComponent Clone(Entity target)
        {
            var component = new TilemapEntityComponent(target)
            {
                _drawableElement = GameInstance.GetService<SceneRenderer>().CreateDrawableElement(target.IsInitializing)
            };
            component._drawableElement.Mesh = _drawableElement.Mesh;
            component._drawableElement.Material = _drawableElement.Material;
            component.IsActive = target.IsActive;
            return component;
        }

        public override void OnComponentRemove()
        {
            GameInstance.GetService<SceneRenderer>().RemoveDrawableElement(_drawableElement);
            _drawableElement = null;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _drawableElement.WorldMatrix = ReferringEntity.WorldMatrix;
            _drawableElement.Scale = ReferringEntity.Scale;
            _drawableElement.GlobalPosition = ReferringEntity.GlobalPosition;

            if (_drawableElement.Mesh != null)
            {
                _drawableElement.UpdateBounds();
            }
        }
    }
}
