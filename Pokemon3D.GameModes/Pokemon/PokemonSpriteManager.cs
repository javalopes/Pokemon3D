using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.Common.FileSystem;
using System.IO;
using Pokemon3D.DataModel.GameMode.Pokemon;

namespace Pokemon3D.GameModes.Pokemon
{
    class PokemonSpriteManager
    {
        private Dictionary<string, Texture2D> _menuTextureCache = new Dictionary<string, Texture2D>();
        private Dictionary<string, Texture2D> _frontTextureCache = new Dictionary<string, Texture2D>();
        private Dictionary<string, Texture2D> _backTextureCache = new Dictionary<string, Texture2D>();
        private Dictionary<string, Texture2D> _overworldTextureCache = new Dictionary<string, Texture2D>();

        private GameMode _gameMode;

        public PokemonSpriteManager(GameMode gameMode)
        {
            _gameMode = gameMode;
        }

        const string KEY_FORMAT = "{0}\\{1}\\{2}";

        private string CreateKey(string pokemonId, string formId, string textureSource)
        {
            return string.Format(KEY_FORMAT, pokemonId, formId, textureSource);
        }

        public Texture2D GetMenuTexture(string pokemonId, string formId, string textureSource)
        {
            string key = CreateKey(pokemonId, formId, textureSource);
            Texture2D texture;

            if (!_menuTextureCache.TryGetValue(key, out texture))
            {
                // load texture and assign to texture variable.

                texture = _gameMode.GameContext.Texture2DProvider.GetTexture2D(textureSource);
                _menuTextureCache.Add(key, texture);
            }

            return texture;
        }
    }
}
