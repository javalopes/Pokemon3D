using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;
using Pokemon3D.Rendering.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokemon3D.Rendering.Compositor
{
    internal class RenderQueue : GameContextObject
    {
        private readonly Action<Material> _handleEffect;
        private readonly Func<IList<DrawableElement>> _getDrawableElements;

        protected SceneEffect SceneEffect { get; }
        public BlendState BlendState { get; set; }
        public RasterizerState RasterizerState { get; set; }
        public DepthStencilState DepthStencilState { get; set; }
        public bool SortNodesBackToFront { get; set; }
        public bool IsEnabled { get; set; }

        public RenderQueue(GameContext context, 
                           Action<Material> handleEffect,
                           Func<IList<DrawableElement>> getDrawableElements,
                           SceneEffect sceneEffect) : base(context)
        {
            _handleEffect = handleEffect;
            _getDrawableElements = getDrawableElements;
            SceneEffect = sceneEffect;
            IsEnabled = true;
        }

        public void Draw(Camera camera, float yRotationForBillboards)
        {
            GameContext.GraphicsDevice.BlendState = BlendState;
            GameContext.GraphicsDevice.DepthStencilState = DepthStencilState;
            GameContext.GraphicsDevice.RasterizerState = RasterizerState;

            var drawableElements = _getDrawableElements();

            var nodes = SortNodesBackToFront ? drawableElements.OrderByDescending(n => (camera.GlobalPosition - n.GlobalPosition).LengthSquared()).ToList()
                                             : drawableElements;

            for (var i = 0; i < nodes.Count; i++)
            {
                var element = nodes[i];

                if (!IsValidForRendering(camera, element)) continue;
                _handleEffect(element.Material);
                DrawElement(camera, element, yRotationForBillboards);
            }
        }

        protected bool IsValidForRendering(Camera camera, DrawableElement element)
        {
            return element.IsActive && camera.Frustum.Contains(element.BoundingBox) != ContainmentType.Disjoint;
        }

        private void DrawElement(Camera camera, DrawableElement element, float yRotationForBillboards)
        {
            DrawElement(camera, element, SceneEffect, yRotationForBillboards);
        }

        internal static void DrawElement(Camera camera, DrawableElement element, SceneEffect sceneEffect, float yRotationForBillboards)
        {
            sceneEffect.World = element.GetWorldMatrix(camera.GlobalEulerAngles.Y);
            sceneEffect.WorldLight = element.GetWorldMatrix(yRotationForBillboards);
            sceneEffect.DiffuseTexture = element.Material.DiffuseTexture;
            sceneEffect.TexcoordScale = element.Material.TexcoordScale;
            sceneEffect.TexcoordOffset = element.Material.TexcoordOffset;
            sceneEffect.MaterialColor = element.Material.Color;

            for (var i = 0; i < sceneEffect.CurrentTechniquePasses.Count; i++)
            {
                sceneEffect.CurrentTechniquePasses[i].Apply();
                element.Mesh.Draw();
            }
        }
    }
}
