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

        public bool VerticalOrientation { get; set; } = true;

        /// <summary>
        /// The action that occurs when the cursor is moved right.
        /// </summary>
        public Action MoveRight { get; set; } = null;
        /// <summary>
        /// The action that occurs when the cursor is moved up.
        /// </summary>
        public Action MoveUp { get; set; } = null;
        /// <summary>
        /// The action that occurs when the cursor is moved left.
        /// </summary>
        public Action MoveLeft { get; set; } = null;
        /// <summary>
        /// The action that occurs when the cursor is moved down.
        /// </summary>
        public Action MoveDown { get; set; } = null;

        public DefaultControlGroup()
        {
            _batch = new SpriteBatch(Game.GraphicsDevice);
        }

        public override void Update()
        {
            if (Active)
            {
                if (Game.InputSystem.Up(true, DirectionalInputTypes.All))
                    if (MoveUp != null)
                        MoveUp();
                    else if (VerticalOrientation)
                        MoveSelection(-1);
                if (Game.InputSystem.Down(true, DirectionalInputTypes.All))
                    if (MoveDown != null)
                        MoveDown();
                    else if (VerticalOrientation)
                        MoveSelection(1);
                if (Game.InputSystem.Left(true, DirectionalInputTypes.All))
                    if (MoveLeft != null)
                        MoveLeft();
                    else if (!VerticalOrientation)
                        MoveSelection(-1);
                if (Game.InputSystem.Right(true, DirectionalInputTypes.All))
                    if (MoveRight != null)
                        MoveRight();
                    else if (!VerticalOrientation)
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
