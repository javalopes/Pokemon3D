using Pokemon3D.Common.Diagnostics;
using Pokemon3D.Common.Localization;
using Pokemon3D.DataModel;
using Pokemon3D.DataModel.i18n;
using Pokemon3D.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.GameCore
{
    internal class TranslationProviderImp : TranslationProvider
    {
        private const string I18NFileExtension = ".json";
        private const string KeyFormat = "{0}>{1}>{2}";
        private readonly Dictionary<string, string> _translations = new Dictionary<string, string>();

        public event EventHandler LanguageChanged;

        public TranslationProviderImp()
        {
            GameController.Instance.GetService<GameConfiguration>().ConfigFileLoaded += OnConfigFileLoaded;

            if (Directory.Exists(I18NPathProvider.LookupPath))
            {
                List<SectionModel> sectionModels = new List<SectionModel>();

                var files = Directory.GetFiles(I18NPathProvider.LookupPath).
                    Where(m => m.EndsWith(I18NFileExtension, StringComparison.OrdinalIgnoreCase));

                foreach (var file in files)
                {
                    try
                    {
                        sectionModels.AddRange(DataModel<SectionModel[]>.FromFile(file));
                    }
                    catch (DataLoadException)
                    {
                        GameLogger.Instance.Log(MessageType.Error, "Error trying to load internationalization file (" + file + ").");
                    }
                }

                Load(sectionModels.ToArray());
            }
            else
            {
                GameLogger.Instance.Log(MessageType.Warning, "The internationalization folder (\"i18n\") was not found.");
            }
        }

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

        private void OnConfigFileLoaded(object sender, EventArgs e)
        {
            OnLanguageChanged(sender, e);
        }

        public void OnLanguageChanged(object sender, EventArgs e)
        {
            LanguageChanged?.Invoke(this, e);
        }

        public string GetTranslation(string sectionId, string tokenId)
        {
            var key = string.Format(KeyFormat, GameInstance.GetService<GameConfiguration>().Data.DisplayLanguage, sectionId, tokenId);
            string value;
            if (_translations.TryGetValue(key, out value)) return value;

            return $"@{sectionId}:{tokenId}";
        }

        public LocalizedValue CreateValue(string sectionId, string tokenId)
        {
            return new LocalizedValue(this, sectionId, tokenId);
        }
    }
}
