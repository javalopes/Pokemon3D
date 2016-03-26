using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Pokemon3D.Common.Shapes;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.GameCore;
using System.Diagnostics;

namespace Pokemon3D.Screens.GameMenu
{
    class GameMenuScreen : GameObject, Screen
    {
        private Pie2D _pie;
        private float _cPie = 0f;

        public void OnOpening(object enterInformation)
        {
            _pie = new Pie2D(Game.GraphicsDevice, 100, _cPie, 40, new Vector2(0, 0), false);
        }

        public void OnDraw(GameTime gameTime)
        {
            if (Common.GlobalRandomProvider.Instance.Rnd.Next(0, 20) == 0)
                _cPie = (float)Pokemon3D.Common.GlobalRandomProvider.Instance.Rnd.NextDouble() * MathHelper.TwoPi;

            _pie.Angle = MathHelper.Lerp(_pie.Angle, _cPie, 0.1f);

            _pie.Draw();
        }

        public void OnUpdate(float elapsedTime)
        {

        }

        public void OnClosing()
        {

        }
    }
}
