using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;
using System.Collections.Generic;
using System.Linq;
using Pokemon3D.Rendering.Data;

// ReSharper disable once ForCanBeConvertedToForeach
namespace Pokemon3D.Rendering.Compositor
{

    internal class RenderQueue : GameContextObject
    {
        private readonly GraphicsDevice _device;

        protected EffectProcessor EffectProcessor { get; }
        public BlendState BlendState { get; set; }
        public RasterizerState RasterizerState { get; set; }
        public DepthStencilState DepthStencilState { get; set; }
        public bool SortNodesBackToFront { get; set; }

        public List<DrawableElement> Elements { get; }
            
        public bool IsEnabled { get; set; }

        public RenderQueue(GameContext context, EffectProcessor effectProcessor) : base(context)
        {
            EffectProcessor = effectProcessor;
            IsEnabled = true;
            _device = context.GetService<GraphicsDevice>();
            Elements = new List<DrawableElement>();
        }

        public void Draw(Camera camera, RenderSettings renderSettings, float yRotationForBillboards)
        { 
            if (Elements.Count == 0) return;

            _device.BlendState = BlendState;
            _device.DepthStencilState = camera.DepthStencilState ?? DepthStencilState;
            _device.RasterizerState = RasterizerState;
            
            var nodes = SortNodesBackToFront ? Elements.OrderByDescending(n => (camera.GlobalPosition - n.GlobalPosition).LengthSquared()).ToList()
                                             : Elements;
            
            for (var i = 0; i < nodes.Count; i++)
            {
                var element = nodes[i];

                if ((element.CameraMask & camera.CameraMask) != camera.CameraMask) continue;
                if (!IsValidForRendering(camera, element)) continue;

                DrawElement(camera, element, EffectProcessor, renderSettings, yRotationForBillboards);
            }
            
        }

        private static bool IsValidForRendering(Camera camera, DrawableElement element)
        {
            return element.IsActive && (!camera.UseCulling || camera.Frustum.Contains(element.BoundingBox) != ContainmentType.Disjoint);
        }

        protected virtual EffectPassCollection GetPasses(Material material, RenderSettings renderSettings)
        {
            return EffectProcessor.ApplyByMaterial(material, renderSettings);
        }

        private void DrawElement(Camera camera, DrawableElement element, EffectProcessor effectProcessor, RenderSettings renderSettings, float yRotationForBillboards)
        {
            effectProcessor.World = element.GetWorldMatrix(camera.GlobalEulerAngles.Y);
            effectProcessor.WorldLight = element.GetWorldMatrix(yRotationForBillboards);

            var passes = GetPasses(element.Material, renderSettings);

            for (var i = 0; i < passes.Count; i++)
            {
                passes[i].Apply();
                element.Mesh.Draw();
            }
        }
    }
}
