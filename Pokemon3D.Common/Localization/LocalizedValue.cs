using System;

namespace Pokemon3D.Common.Localization
{
    public class LocalizedValue
    {
        private string _key;
        private string _section;
        private TranslationProvider _translationprovider;

        public event Action ValueChanged;

        public LocalizedValue(TranslationProvider translationProvider, string section, string key)
        {
            _translationprovider = translationProvider;
            _section = section;
            _key = key;

            _translationprovider.LanguageChanged += OnLanguageChanged;
            Value = _translationprovider.GetTranslation(_section, _key);
        }

        public void UnSubscribe()
        {
            _translationprovider.LanguageChanged -= OnLanguageChanged;
        }

        private void OnLanguageChanged(object sender, EventArgs e)
        {
            Value = _translationprovider.GetTranslation(_section, _key);
            ValueChanged?.Invoke();
        }

        public string Value { get; private set; }
    }
}
