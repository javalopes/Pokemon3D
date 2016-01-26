using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Pokemon3D.UI.Framework;
using Pokemon3D.GameCore;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.Common.Input;

namespace Pokemon3D.UI.Screens
{
    class OverlayScreen : GameObject, Screen
    {
        private Screen _preScreen;
        private HexagonBackground _hexagons;
        private ControlBar _bar;
        
        private DefaultControlGroup _pokemonProfiles;

        public void OnOpening(object enterInformation)
        {
            _preScreen = (Screen)enterInformation;
            _hexagons = new HexagonBackground();
            _bar = new ControlBar();

            _bar.AddEntry("Select", Buttons.A, Keys.Enter);
            _bar.AddEntry("Back", Buttons.B, Keys.Escape);
            
            _pokemonProfiles = new DefaultControlGroup();
            
            for (int i = 0; i < Game.LoadedSave.PartyPokemon.Count; i++)
            {
                var pokemon = Game.LoadedSave.PartyPokemon[i];
                _pokemonProfiles.Add(new PokemonProfile(Game.ActiveGameMode, pokemon, new Vector2(110 * i + 280, 80 + ((i % 2) * 52))));
            }
            
            _pokemonProfiles.Active = true;
            _pokemonProfiles.Visible = true;
            _pokemonProfiles.VerticalOrientation = false;
        }

        public void OnDraw(GameTime gameTime)
        {
            _preScreen.OnDraw(gameTime);

            _hexagons.Draw();

            _bar.Draw();

            _pokemonProfiles.Draw();
        }
        
        public void OnUpdate(float elapsedTime)
        {
            _hexagons.Update();
            
            if (Game.InputSystem.Dismiss(DismissInputTypes.BButton | DismissInputTypes.EscapeKey))
            {
                Game.ScreenManager.SetScreen(_preScreen.GetType());
            }

            _pokemonProfiles.Update();
        }

        public void OnClosing()
        {

        }
    }
}
