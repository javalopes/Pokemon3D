using Pokemon3D.DataModel;
using Pokemon3D.DataModel.i18n;
using Pokemon3D.UI.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pokemon3D.Entities.Resources
{
    class GameModei18nManager : TranslationManager
    {
        private GameMode _gameMode;

        public GameModei18nManager(GameMode gameMode)
        {
            _gameMode = gameMode;

            if (Directory.Exists(_gameMode.i18nPath))
            {
                List<SectionModel> sectionModels = new List<SectionModel>();

                var files = Directory.GetFiles(_gameMode.i18nPath).
                    Where(m => m.EndsWith(I18NFileExtension, StringComparison.OrdinalIgnoreCase));

                foreach (var file in files)
                {
                    try
                    {
                        sectionModels.AddRange(DataModel<SectionModel[]>.FromFile(file));
                    }
                    catch (DataLoadException)
                    {
                        // todo: log exception
                    }
                }

                Load(sectionModels.ToArray());
            }
            else
            {
                // todo: log, that the folder does not exist.
            }
        }

        #region Dispose

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // todo: free managed resources
            }

            // todo: free unmanaged resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Add, if this class has unmanaged resources
        //~GameModei18nManager()
        //{
        //    // Destructor calls dipose to free unmanaged resources:
        //    Dispose(false);
        //}

        #endregion
    }
}
