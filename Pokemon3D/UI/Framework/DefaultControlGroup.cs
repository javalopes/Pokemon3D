using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.UI.Framework
{
    /// <summary>
    /// A default control group which manages moving between controls.
    /// </summary>
    class DefaultControlGroup : ControlGroup
    {
        private SpriteBatch _batch;

        public DefaultControlGroup()
        {
            _batch = new SpriteBatch(Game.GraphicsDevice);
        }

        public override void Update()
        {
            if (Active)
            {
                if (Game.InputSystem.Up(true, DirectionalInputTypes.All))
                    MoveSelection(-1);
                if (Game.InputSystem.Down(true, DirectionalInputTypes.All))
                    MoveSelection(1);
            }

            base.Update();
        }

        public override void Draw()
        {
            _batch.Begin(samplerState: SamplerState.PointWrap, blendState: BlendState.AlphaBlend);
            InternalDraw(_batch);
            _batch.End();
        }
    }
}
