using Pokemon3D.Common.Localization;
using Pokemon3D.DataModel.i18n;
using Pokemon3D.GameCore;
using System;
using System.Collections.Generic;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.UI.Localization
{
    /// <summary>
    /// The base class for internationalization Managers.
    /// </summary>
    abstract class TranslationManager : TranslationProvider
    {
        protected const string I18NFileExtension = ".json";
        private const string KeyFormat = "{0}>{1}>{2}";

        private readonly Dictionary<string, string> _translations = new Dictionary<string, string>();

        public event EventHandler LanguageChanged;

        /// <summary>
        /// Processes the loaded section models into tokens.
        /// </summary>
        protected void Load(SectionModel[] sectionModels)
        {
            foreach (var section in sectionModels)
            {
                foreach (var token in section.Tokens)
                {
                    _translations.Add(string.Format(KeyFormat, section.Language, section.Id, token.Id), token.Val);
                }
            }
        }

        public string GetTranslation(string sectionId, string tokenId)
        {
            var key = string.Format(KeyFormat, GameInstance.GetService<GameConfiguration>().DisplayLanguage, sectionId, tokenId);
            string value;
            if (_translations.TryGetValue(key, out value)) return value;

            return string.Format("@{0}:{1}", sectionId, tokenId);
        }

        public void OnLanguageChanged(object sender, EventArgs e)
        {
            LanguageChanged?.Invoke(this, e);
        }
    }
}
