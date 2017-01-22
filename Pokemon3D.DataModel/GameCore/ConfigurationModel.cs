﻿using System.Globalization;
using System.Runtime.Serialization;
using Pokemon3D.DataModel.General;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.GameCore
{
    [DataContract(Namespace = "")]
    public class ConfigurationModel : DataModel<ConfigurationModel>
    {
        [DataMember(Order = 0)]
        public string DisplayLanguage;

        [DataMember(Order = 1)]
        public int MusicVolume;

        [DataMember(Order = 2)]
        public int SoundVolume;

        [DataMember(Order = 3)]
        public SizeModel WindowSize;

        [DataMember(Order = 4)]
        public bool ShadowsEnabled;

        [DataMember(Order = 5)]
        public int ShadowMapSize;

        [DataMember(Order = 6)]
        public bool SoftShadows;

        [DataMember(Order = 7)]
        public bool EnableFileHotSwapping;

        [DataMember(Order = 8)]
        public string CustomGameModeBasePath;

        [DataMember(Order = 9)]
        public InputActionModel[] InputActions;

        public static ConfigurationModel Default => new ConfigurationModel
        {
            DisplayLanguage = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName,
            MusicVolume = 75,
            SoundVolume = 100,
            ShadowsEnabled = true,
            SoftShadows = true,
            ShadowMapSize = 1024,
            WindowSize = new SizeModel
            {
                Width = 1024,
                Height = 600
            },
            EnableFileHotSwapping = false,
            CustomGameModeBasePath = @"..\..\..\..\GameModeTemplates"
        };

        public override object Clone()
        {
            var clone = (ConfigurationModel)MemberwiseClone();
            clone.WindowSize = WindowSize.CloneModel();
            return clone;
        }
    }
}
