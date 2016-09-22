using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Data;
using Pokemon3D.Common;
using Pokemon3D.Content;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.Collisions
{
    internal class CollisionManager
    {
        private readonly object _lockObject = new object();

        private readonly List<Collider> _allColliders;
        private readonly List<Collider> _allTriggers; 
        private readonly List<Collider> _allTriggersAndColliders;

        private readonly Mesh _boundingBoxMesh;
        private readonly List<CollisionResult> _colliderList = new List<CollisionResult>();

        private readonly Effect _lineDrawEffect;
        private readonly EffectParameter _worldViewProjection;
        private readonly EffectParameter _modulateColor;
        private readonly EffectTechnique _lineTechnique;

        public bool DrawDebugShapes { get; set; }

        public CollisionManager()
        {
            _allColliders = new List<Collider>();
            _allTriggers = new List<Collider>();
            _allTriggersAndColliders = new List<Collider>();

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

            _boundingBoxMesh = new Mesh(GameInstance.GraphicsDevice, geometryBox, PrimitiveType.LineList, false)
            {
                PreventDrawCallCount = true
            };

            _lineDrawEffect = GameInstance.Content.Load<Effect>(ResourceNames.Effects.DebugShadowMap);
            _worldViewProjection = _lineDrawEffect.Parameters["WorldViewProjection"];
            _modulateColor = _lineDrawEffect.Parameters["Color"];
            _lineTechnique = _lineDrawEffect.Techniques["LineDraw"];
        }

        public void Add(Collider collider)
        {
            lock (_lockObject)
            {
                _allTriggersAndColliders.Add(collider);

                if (collider.IsTrigger)
                {
                    _allTriggers.Add(collider);
                }
                else
                {
                    _allColliders.Add(collider);
                }
            }
        }

        public void Remove(Collider collider)
        {
            lock (_lockObject)
            {
                _allTriggersAndColliders.Remove(collider);

                if (collider.IsTrigger)
                {
                    _allTriggers.Remove(collider);
                }
                else
                {
                    _allColliders.Remove(collider);
                }
            }
        }

        public CollisionResult[] CheckCollision(Collider collider)
        {
            lock (_lockObject)
            {
                _colliderList.Clear();
                foreach (var possibleCollider in _allColliders)
                {
                    if (collider == possibleCollider) continue;
                    if (!possibleCollider.IsActive) continue;

                    var result = possibleCollider.CheckCollision(collider);
                    if (result.Collides)
                    {
                        _colliderList.Add(result);
                    }
                }

                return _colliderList.ToArray();
            }
        }

        public void Update()
        {
            lock (_lockObject)
            {
                foreach (var trigger in _allTriggers)
                {
                    if (!trigger.IsActive) continue;

                    foreach (var collidingPartner in _allTriggersAndColliders)
                    {
                        if (trigger == collidingPartner) continue;
                        if (!collidingPartner.IsActive) continue;

                        if (trigger.Intersects(collidingPartner))
                        {
                            trigger.HandleTrigger(collidingPartner);
                            collidingPartner.HandleTrigger(trigger);
                        }
                    }
                }

                foreach (var collider in _allTriggersAndColliders)
                {
                    collider.HandleUntouched();
                }
            }
        }

        public void Draw(Camera observer)
        {
            if (!DrawDebugShapes) return;

            GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GameInstance.GraphicsDevice.BlendState = BlendState.Opaque;

            var oldTechnique = _lineDrawEffect.CurrentTechnique;
            _lineDrawEffect.CurrentTechnique = _lineTechnique;

            lock (_lockObject)
            {
                for (var i = 0; i < _allTriggersAndColliders.Count; i++)
                {
                    var collider = _allTriggersAndColliders[i];
                    if (!collider.IsActive) continue;
                    switch (collider.Type)
                    {
                        case ColliderType.BoundingBox:
                            DrawBoundingBox(observer, collider);
                            break;
                    }
                }
            }

            _lineDrawEffect.CurrentTechnique = oldTechnique;
        }

        private static readonly Vector4 ColorTrigger = new Vector4(0.2f, 1.0f, 0.1f, 1.0f);
        private static readonly Vector4 ColorCollider = new Vector4(1.0f, 0.1f, 0.1f, 1.0f);

        private void DrawBoundingBox(Camera observer, Collider collider)
        {
            var scale = -collider.BoundingBox.Min + collider.BoundingBox.Max;
            var position = collider.BoundingBox.Min + scale*0.5f;
            var worldViewProjection = Matrix.CreateScale(scale)*Matrix.CreateTranslation(position)*observer.ViewMatrix*
                                      observer.ProjectionMatrix;

            _worldViewProjection.SetValue(worldViewProjection);
            _modulateColor.SetValue(collider.IsTrigger ? ColorTrigger : ColorCollider);

            _lineDrawEffect.CurrentTechnique.Passes[0].Apply();
            _boundingBoxMesh.Draw();
        }
    }
}
