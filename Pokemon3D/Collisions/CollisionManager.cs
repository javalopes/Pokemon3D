﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Data;
using Pokemon3D.Common;
using Pokemon3D.Content;
using static Pokemon3D.GameProvider;

namespace Pokemon3D.Collisions
{
    internal class CollisionManager
    {
        private readonly object _lockObject = new object();

        private readonly List<Collider> _allColliders;
        private readonly List<Collider> _allTriggers; 
        private readonly List<Collider> _allTriggersAndColliders;

        private readonly Mesh _boundingBoxMesh;
        private readonly Material _material;
        private readonly List<CollisionResult> _colliderList = new List<CollisionResult>();

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
            _material = new Material
            {
                Color = Color.White,
                UseLinearTextureSampling = false,
                ReceiveShadow = false,
                CastShadow = false,
                UseTransparency = false,
                IsUnlit = true
            };

            GameInstance.GetService<SceneRenderer>().RegisterCustomDraw(OnCustomDraw);
        }

        private void OnCustomDraw(Camera camera, SceneRenderer renderer)
        {
            if (!DrawDebugShapes) return;

            GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GameInstance.GraphicsDevice.BlendState = BlendState.Opaque;

            lock (_lockObject)
            {
                for (var i = 0; i < _allTriggersAndColliders.Count; i++)
                {
                    var collider = _allTriggersAndColliders[i];
                    if (!collider.IsActive) continue;
                    switch (collider.Type)
                    {
                        case ColliderType.BoundingBox:
                            DrawBoundingBox(renderer, camera, collider);
                            break;
                    }
                }
            }
            
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

        private static readonly Color ColorTrigger = new Color(25, 255, 25, 255); 
        private static readonly Color ColorCollider = new Color(255, 25,25,255);       

        private void DrawBoundingBox(SceneRenderer renderer, Camera camera, Collider collider)
        {
            var scale = -collider.BoundingBox.Min + collider.BoundingBox.Max;
            var position = collider.BoundingBox.Min + scale*0.5f;
            _material.Color = collider.IsTrigger ? ColorTrigger : ColorCollider;

            renderer.DrawImmediate(camera, Matrix.CreateScale(scale) * Matrix.CreateTranslation(position), _material, _boundingBoxMesh);
        }
    }
}
