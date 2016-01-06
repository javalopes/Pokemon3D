using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.GameCore;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Data;
using Pokemon3D.Common;

namespace Pokemon3D.Collisions
{
    class CollisionManager : GameObject
    {
        private readonly List<Collider> _allColliders;
        private Mesh _boundingBoxMesh;

        private readonly Effect _lineDrawEffect;
        private readonly EffectParameter _worldViewProjection;
        private readonly EffectTechnique _lineTechnique;

        public bool DrawDebugShapes { get; set; }

        public CollisionManager()
        {
            _allColliders = new List<Collider>();

            var geometryBox = new GeometryData
            {
                Vertices = new[]
                {
                    new VertexPositionNormalTexture(new Vector3(-0.5f, -0.5f, -0.5f), Vector3.Zero, Vector2.Zero),
                    new VertexPositionNormalTexture(new Vector3( 0.5f, -0.5f, -0.5f), Vector3.Zero, Vector2.Zero),
                    new VertexPositionNormalTexture(new Vector3(0.5f, -0.5f,  0.5f), Vector3.Zero, Vector2.Zero),
                    new VertexPositionNormalTexture(new Vector3(-0.5f, -0.5f, 0.5f), Vector3.Zero, Vector2.Zero),

                    new VertexPositionNormalTexture(new Vector3(-0.5f, 0.5f, -0.5f), Vector3.Zero, Vector2.Zero),
                    new VertexPositionNormalTexture(new Vector3( 0.5f, 0.5f, -0.5f), Vector3.Zero, Vector2.Zero),
                    new VertexPositionNormalTexture(new Vector3(0.5f, 0.5f,  0.5f), Vector3.Zero, Vector2.Zero),
                    new VertexPositionNormalTexture(new Vector3(-0.5f, 0.5f, 0.5f), Vector3.Zero, Vector2.Zero),
                },
                Indices = new ushort[]
                {
                    0,1,1,2,2,3,3,0,
                    4,5,5,6,6,7,7,4,
                    0,4,1,5,2,6,3,7
                }
            };

            _boundingBoxMesh = new Mesh(Game.GraphicsDevice, geometryBox, PrimitiveType.LineList, false);

            _lineDrawEffect = Game.Content.Load<Effect>(ResourceNames.Effects.DebugShadowMap);
            _worldViewProjection = _lineDrawEffect.Parameters["WorldViewProjection"];
            _lineTechnique = _lineDrawEffect.Techniques["LineDraw"];
        }

        public void AddCollider(Collider collider)
        {
            _allColliders.Add(collider);
        }

        public void RemoveCollider(Collider collider)
        {
            _allColliders.Remove(collider);
        }

        private List<CollisionResult> _colliderList = new List<CollisionResult>();

        public CollisionResult[] CheckCollision(Collider collider)
        {
            _colliderList.Clear();
            foreach (var possibleCollider in _allColliders)
            {
                if (collider == possibleCollider) continue;

                var result = possibleCollider.CheckCollision(collider);
                if (result.Collides)
                {
                    _colliderList.Add(result);
                }
            }

            return _colliderList.ToArray();
        }

        public void Draw(Camera observer)
        {
            if (!DrawDebugShapes) return;

            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Game.GraphicsDevice.BlendState = BlendState.Opaque;

            _lineDrawEffect.CurrentTechnique = _lineTechnique;

            for (var i = 0; i < _allColliders.Count; i++)
            {
                var collider = _allColliders[i];
                switch (collider.Type)
                {
                    case ColliderType.BoundingBox:
                        DrawBoundingBox(observer, collider);
                        break;
                    case ColliderType.BoundingSphere:
                        //todo
                        break;
                }
            }
        }

        private void DrawBoundingBox(Camera observer, Collider collider)
        {
            var scale = -collider.BoundingBox.Min + collider.BoundingBox.Max;
            var position = collider.BoundingBox.Min + scale*0.5f;
            var worldViewProjection = Matrix.CreateScale(scale)*Matrix.CreateTranslation(position)*observer.ViewMatrix*
                                      observer.ProjectionMatrix;

            _worldViewProjection.SetValue(worldViewProjection);

            _lineDrawEffect.CurrentTechnique.Passes[0].Apply();

            _boundingBoxMesh.Draw();
        }
    }
}
