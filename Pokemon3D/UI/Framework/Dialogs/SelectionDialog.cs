using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.GameCore;

namespace Pokemon3D.UI.Framework.Dialogs
{
    class SelectionDialog : GameObject
    {
        private string _title;
        private string _text;
        private bool _isVisible;

        SpriteFont _titleFont;
        SpriteFont _textFont;

        private ControlGroup _group;

        public SelectionDialog(string title, string text, LeftSideButton[] buttons, int selectedIndex)
        {
            _group = new ControlGroup();
            _group.AddRange(buttons);
            _group.SetSelection(selectedIndex);
        }

        public void Update()
        {
            _group.Update();
        }

        public void Draw()
        {
            if (_isVisible)
            {
                Game.SpriteBatch.Begin(samplerState: SamplerState.PointWrap, blendState: BlendState.AlphaBlend);

                _group.Draw();

                Game.SpriteBatch.End();
            }
        }

        public void Show()
        {
            _isVisible = true;
        }
    }
}
