using System;

namespace Pokemon3D.Common.Localization
{
    /// <summary>
    /// Implements functionality to get translations from raw text sources.
    /// </summary>
    public interface TranslationProvider
    {
        /// <summary>
        /// Returns a localized string for a resourc ekey.
        /// </summary>
        /// <param name="sectionId">Section</param>
        /// <param name="tokenId">Token</param>
        /// <returns>translated key.</returns>
        string GetTranslation(string sectionId, string tokenId);
        
        /// <summary>
        /// An event that gets fired when the language of the game changes.
        /// </summary>
        event EventHandler LanguageChanged;
    }
}
