using Microsoft.Xna.Framework;
using Pokemon3D.Common.Shapes;
using Pokemon3D.GameCore;
using Pokemon3D.Entities.Pokemon;
using Pokemon3D.UI.Framework;
using System;

namespace Pokemon3D.Screens.GameMenu
{
    class GameMenuScreen : GameObject, Screen
    {
        private ShapeRenderer _renderer;

        public void OnOpening(object enterInformation)
        {
            _renderer = new ShapeRenderer(Game.SpriteBatch);
        }

        public void OnEarlyDraw(GameTime gameTime)
        {        }

		public void OnLateDraw(GameTime gameTime)
        {
			Game.SpriteBatch.Begin();



            _renderer.DrawShapeGradientFill(new Plane2D(0, 0, 100, 100), null, Color.White, Color.Black, true);

            Game.SpriteBatch.End();
        }

        public void OnUpdate(float elapsedTime)
        {

        }

        public void OnClosing()
        {

        }
    }

    class PokemonProfile : GameObject
    {
        private Pokemon _pokemon;
        private ShapeRenderer _renderer;
        private PokemonSpriteSheet _sheet;
        private Pie2D _pie, _backPie;
        private float _pieValue = MathHelper.TwoPi;
        private ColorTransition _colorTransition;
        private Vector2 _position;

        public PokemonProfile(Pokemon pokemon, ShapeRenderer renderer, Vector2 position)
        {
            _pokemon = pokemon;
            _renderer = renderer;
            _position = position;
            var dataModel = _pokemon.ActiveFormModel.FrontSpriteSheet;
            _sheet = new PokemonSpriteSheet(Game.ActiveGameMode.GetTexture(dataModel.Source), dataModel.FrameSize.Width, dataModel.FrameSize.Height);
            _pie = new Pie2D(Game.GraphicsDevice, 80, MathHelper.ToRadians(360), 40, _position, false);
            _backPie = new Pie2D(Game.GraphicsDevice, 75, MathHelper.TwoPi, 40, new Vector2(5) + _position, false);
            _backPie.SecondaryColor = Color.Black;
            _backPie.ChartType = PieChartType.RadialFill;

            _pie.PrimaryColor = Color.Green;
            _pie.ChartType = PieChartType.RadialFill;

            _colorTransition = new ColorTransition(Color.Green, 0.8f);
        }

        public void Draw()
        {
            _backPie.DrawBatched(Game.SpriteBatch);
            _pie.DrawBatched(Game.SpriteBatch);
            _renderer.DrawShape(new Ellipse((int)(24 + _position.X), (int)(24 + _position.Y), 112, 112), Color.LightBlue);

            int width = Math.Min(_sheet.CurrentFrame.Width, 128);
            int height = Math.Min(_sheet.CurrentFrame.Height, 128);
            
            _renderer.Batch.Draw(_sheet.CurrentFrame, new Rectangle((int)(16 + _position.X + (128 - width) / 2), (int)(16 + _position.Y + (128 - height) / 2), width, height), Color.White);
        }

        public void Update()
        {
            _sheet.Update();

            if (Common.GlobalRandomProvider.Instance.Rnd.Next(0, 50) == 0)
            {
                _pieValue = (float)(Common.GlobalRandomProvider.Instance.Rnd.NextDouble() * MathHelper.TwoPi);
            }

            _pie.Angle = MathHelper.Lerp(_pie.Angle, _pieValue, 0.1f);

            if (_pieValue > MathHelper.Pi)
                _colorTransition.TargetColor = Color.Green;
            else if (_pieValue > MathHelper.TwoPi * 0.15f)
                _colorTransition.TargetColor = Color.Gold;
            else
                _colorTransition.TargetColor = Color.Red;

            _colorTransition.Update();
            _pie.PrimaryColor = _colorTransition.Color;
        }
    }
}
