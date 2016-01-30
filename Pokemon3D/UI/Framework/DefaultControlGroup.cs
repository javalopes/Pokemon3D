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

        public ControlGroupOrientation Orientation { get; set; } = ControlGroupOrientation.Vertical;

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
                if (Game.InputSystem.Up(InputDetectionType.PressedOnce, DirectionalInputTypes.All))
                    if (MoveUp != null)
                        MoveUp();
                    else if (Orientation.HasFlag(ControlGroupOrientation.Vertical))
                        MoveSelection(-1);
                if (Game.InputSystem.Down(InputDetectionType.PressedOnce, DirectionalInputTypes.All))
                    if (MoveDown != null)
                        MoveDown();
                    else if (Orientation.HasFlag(ControlGroupOrientation.Vertical))
                        MoveSelection(1);
                if (Game.InputSystem.Left(InputDetectionType.PressedOnce, DirectionalInputTypes.All))
                    if (MoveLeft != null)
                        MoveLeft();
                    else if (Orientation.HasFlag(ControlGroupOrientation.Horizontal))
                        MoveSelection(-1);
                if (Game.InputSystem.Right(InputDetectionType.PressedOnce, DirectionalInputTypes.All))
                    if (MoveRight != null)
                        MoveRight();
                    else if (Orientation.HasFlag(ControlGroupOrientation.Horizontal))
                        MoveSelection(1);
            }

            base.Update();
        }

        public override void Draw(SamplerState samplerState = null, BlendState blendState = null)
        {
            if (samplerState == null)
                samplerState = SamplerState.PointWrap;
            if (blendState == null)
                blendState = BlendState.AlphaBlend;

            _batch.Begin(samplerState: samplerState, blendState: blendState);
            InternalDraw(_batch);
            _batch.End();
        }
    }
}
