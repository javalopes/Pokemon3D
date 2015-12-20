
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;
using Pokemon3D.DataModel.Json.GameCore;
using Pokemon3D.Rendering.GUI;
using Pokemon3D.UI.Transitions;
using Button = Pokemon3D.Rendering.GUI.Button;
using HorizontalAlignment = Pokemon3D.Rendering.GUI.HorizontalAlignment;

namespace Pokemon3D.UI.Screens
{
    class MainMenuScreen : InitializeableScreen
    {
        private Sprite _headerSprite;
        private SpriteText _versionInformation;

        private GuiPanel _mainMenuPanel;
        private GuiPanel _optionsMenuPanel;

        private TextBlock _shadowSizeTextBlock;
        private CheckBox _shadowsEnabledCheckBox;
        private CheckBox _softShadowsCheckBox;

        private readonly string[] _shadowSizesResourceKeys = {
            "SmallShadowMap", "MediumShadowMap",  "LargeShadowMap"
        };

        private static readonly LookupDictionary<int, ShadowQuality> ShadowQualityLookup = new LookupDictionary<int, ShadowQuality>(
            new[]
            {
                new KeyValuePair<int, ShadowQuality>(0, ShadowQuality.Small), 
                new KeyValuePair<int, ShadowQuality>(1, ShadowQuality.Medium), 
                new KeyValuePair<int, ShadowQuality>(2, ShadowQuality.Large)
            });

        private int _shadowSizeIndex = 1;

        protected override void OnInitialize(object enterInformation)
        {
            _headerSprite = new Sprite(Game.Content.Load<Texture2D>(ResourceNames.Textures.P3D))
            {
                Scale = new Vector2(0.5f)
            };
            _headerSprite.Position = new Vector2(Game.ScreenBounds.Width * 0.5f, _headerSprite.Bounds.Height * 0.5f + 10.0f);
            _versionInformation = new SpriteText(Game.Content.Load<SpriteFont>(ResourceNames.Fonts.NormalFont))
            {
                Color = Color.White,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Text = Game.VersionInformation
            };
            _versionInformation.SetTargetRectangle(new Rectangle(0,0, Game.ScreenBounds.Width-10, Game.ScreenBounds.Height - 10));
            
            var guiSpace = new Rectangle(0,50, Game.ScreenBounds.Width, Game.ScreenBounds.Height-30);
            
            var root = Game.GuiSystem.CreateGuiHierarchyFromXml<GuiElement>("Content/Gui/MainMenu.xml");
            _mainMenuPanel = new GuiPanel(Game, root, guiSpace);

            root.FindGuiElementById<Button>("StartButton").Click += OnStartClick;
            root.FindGuiElementById<Button>("LoadButton").Click += OnLoadClick;
            root.FindGuiElementById<Button>("OptionsButton").Click += OnOptionsClick;
            root.FindGuiElementById<Button>("QuitButton").Click += OnQuitClick;

            root = Game.GuiSystem.CreateGuiHierarchyFromXml<GuiElement>("Content/Gui/OptionsMenu.xml");
            root.FindGuiElementById<Button>("BackToMainMenuButton").Click += OnBackToMainMenuButtonClick;

            _shadowSizeTextBlock = root.FindGuiElementById<TextBlock>("ShadowSizeText");
            _shadowsEnabledCheckBox = root.FindGuiElementById<CheckBox>("EnableShadowsCheckbox");
            _softShadowsCheckBox = root.FindGuiElementById<CheckBox>("EnableSoftShadowsCheckbox");
            root.FindGuiElementById<Button>("IncreaseShadowSizeButton").Click += OnIncreaseShadowSize;
            root.FindGuiElementById<Button>("DecreaseShadowSizeButton").Click += OnDecreaseShadowSize;
            root.FindGuiElementById<Button>("SaveSettingsButton").Click += SaveSettingsClicked;

            _optionsMenuPanel = new GuiPanel(Game, root, guiSpace) { IsEnabled = false };
        }

        public override void OnOpening(object enterInformation)
        {
            base.OnOpening(enterInformation);

            var config = Game.GameConfig;

            _shadowSizeIndex = ShadowQualityLookup.Convert(config.ShadowQuality);
            UpdateShadowMapSizeText();

            _shadowsEnabledCheckBox.IsChecked = config.ShadowsEnabled;
            _softShadowsCheckBox.IsChecked = config.SoftShadows;
        }

        private void SaveSettingsClicked()
        {
            var config = Game.GameConfig;

            config.ShadowQuality = ShadowQualityLookup.Convert(_shadowSizeIndex);
            config.ShadowsEnabled = _shadowsEnabledCheckBox.IsChecked;
            config.SoftShadows = _softShadowsCheckBox.IsChecked;
        }

        private void UpdateShadowMapSizeText()
        {
            _shadowSizeTextBlock.Text = Game.TranslationProvider.GetTranslation("System", _shadowSizesResourceKeys[_shadowSizeIndex]);
        }

        private void OnDecreaseShadowSize()
        {
            if (--_shadowSizeIndex < 0) _shadowSizeIndex = _shadowSizesResourceKeys.Length - 1;
            UpdateShadowMapSizeText();
        }

        private void OnIncreaseShadowSize()
        {
            if (++_shadowSizeIndex == _shadowSizesResourceKeys.Length) _shadowSizeIndex = 0;
            UpdateShadowMapSizeText();
        }

        private void OnBackToMainMenuButtonClick()
        {
            _optionsMenuPanel.IsEnabled = false;
            _mainMenuPanel.IsEnabled = true;
        }

        private void OnOptionsClick()
        {
            _optionsMenuPanel.IsEnabled = true;
            _mainMenuPanel.IsEnabled = false;
        }

        private void OnLoadClick()
        {
            
        }

        public override void OnDraw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Black);
            Game.SpriteBatch.Begin();

            _headerSprite.Draw(Game.SpriteBatch);
            _versionInformation.Draw(Game.SpriteBatch);

            _mainMenuPanel.Draw();
            _optionsMenuPanel.Draw();
            Game.SpriteBatch.End();
        }

        public override void OnUpdate(float elapsedTime)
        {
            _mainMenuPanel.Update(elapsedTime);
            _optionsMenuPanel.Update(elapsedTime);
        }

        public override void OnClosing()
        {
        }

        private void OnQuitClick()
        {
            Game.ScreenManager.NotifyQuitGame();
        }

        private void OnStartClick()
        {
            Game.ScreenManager.SetScreen(typeof(OverworldScreen), typeof(SlideTransition));
        }
    }
}
