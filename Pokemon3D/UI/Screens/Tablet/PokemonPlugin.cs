using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Pokemon3D.UI.Framework;
using Pokemon3D.UI.Framework.Tablet;
using Pokemon3D.Common.Shapes;

namespace Pokemon3D.UI.Screens.Tablet
{
    class PokemonPlugin : TabletPlugin
    {
        private RenderTarget2D _target;
        private DefaultControlGroup _buttons;
        private SpriteBatch _batch;
        private ShapeRenderer _renderer;
        private Polygon _statsBack, _EVstats, _IVstats;

        public override string Title
        {
            get { return "Pokemon"; }
        }

        public PokemonPlugin(TabletScreen screen) : base(screen)
        {
            _buttons = new DefaultControlGroup();
            for (int i = 0; i < Game.LoadedSave.PartyPokemon.Count; i++)
                _buttons.Add(new PokemonProfile(Game.ActiveGameMode, Game.LoadedSave.PartyPokemon[i], new Vector2(100 + i * 110, 260 + (i % 2) * 55)));

            _buttons.Visible = true;
            _buttons.Active = true;

            _batch = new SpriteBatch(Game.GraphicsDevice);

            _target = new RenderTarget2D(Game.GraphicsDevice, TabletScreen.TABLET_TARGET_WIDTH, TabletScreen.TABLET_TARGET_HEIGHT);

            _renderer = new ShapeRenderer(_batch);
            _statsBack = new Polygon(new[]
            {
                new Point(55, 0),
                new Point(110, 30),
                new Point(110, 88),
                new Point(55, 118),
                new Point(0, 88),
                new Point(0, 30)
            });

            var center = new Point(55, 59);
            var EVstats = Game.LoadedSave.PartyPokemon[0].EVs;
            var IVstats = Game.LoadedSave.PartyPokemon[0].IVs;

            _EVstats = new Polygon(new[]
            {
                GetMidPoint(center, _statsBack.Points[0], EVstats.HP, 255),
                GetMidPoint(center, _statsBack.Points[1], EVstats.Atk, 255),
                GetMidPoint(center, _statsBack.Points[2], EVstats.Def, 255),
                GetMidPoint(center, _statsBack.Points[3], EVstats.Speed, 255),
                GetMidPoint(center, _statsBack.Points[4], EVstats.SpDef, 255),
                GetMidPoint(center, _statsBack.Points[5], EVstats.SpAtk, 255)
            });
            _IVstats = new Polygon(new[]
            {
                GetMidPoint(center, _statsBack.Points[0], IVstats.HP, 31),
                GetMidPoint(center, _statsBack.Points[1], IVstats.Atk, 31),
                GetMidPoint(center, _statsBack.Points[2], IVstats.Def, 31),
                GetMidPoint(center, _statsBack.Points[3], IVstats.Speed, 31),
                GetMidPoint(center, _statsBack.Points[4], IVstats.SpDef, 31),
                GetMidPoint(center, _statsBack.Points[5], IVstats.SpAtk, 31)
            });
        }

        private Point GetMidPoint(Point center, Point end, int value, int max)
        {
            float part = value / (float)max;
            part *= 0.9f;
            part += 0.1f;

            int resX = (int)(center.X + (end.X - center.X) * part);
            int resY = (int)(center.Y + (end.Y - center.Y) * part);
            return new Point(resX, resY);
        }

        public override Texture2D Draw()
        {
            var previousTargets = Game.GraphicsDevice.GetRenderTargets();

            Game.GraphicsDevice.SetRenderTarget(_target);
            Game.GraphicsDevice.Clear(Color.Transparent);

            _batch.Begin(blendState: BlendState.AlphaBlend);
            
            _renderer.DrawShape(_statsBack, new Vector2(400, 400), Color.LightBlue);
            //_renderer.DrawShape(_EVstats, new Vector2(400 + _EVstats.Bounds.X, 400 + _EVstats.Bounds.Y), Color.DarkGreen);
            _renderer.DrawShape(_IVstats, new Vector2(400 + _IVstats.Bounds.X, 400 + _IVstats.Bounds.Y), Color.LightGreen);
            _renderer.DrawOutline(_EVstats, new Vector2(400 , 400), Color.DarkGreen);

            _buttons.Draw(blendState: BlendState.NonPremultiplied);

            _batch.End();

            Game.GraphicsDevice.SetRenderTargets(previousTargets);

            return _target;
        }

        public override void Update()
        {
            _buttons.Update();
        }
    }
}
