
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.Common.Input;
using Pokemon3D.Common.Localization;
using Pokemon3D.Entities.System;
using Pokemon3D.Entities.System.Components;
using Pokemon3D.Rendering.UI;
using Pokemon3D.UI;
using static GameProvider;

namespace Pokemon3D.Entities
{
    class Inventory
    {
        private readonly Entity _inventoryOverlayEntity;
        private GraphicsDevice _device;
        private WorldUiOverlayEntityComponent _overlayComponent;
        private UiOverlay _uiOverlay;
        private SpriteBatch _spriteBatch;

        public Inventory(World world)
        {
            _device = GameInstance.GetService<GraphicsDevice>();
            _spriteBatch = new SpriteBatch(_device);

            _inventoryOverlayEntity = world.EntitySystem.CreateEntity(true);
            _overlayComponent = _inventoryOverlayEntity.AddComponent(new WorldUiOverlayEntityComponent(_inventoryOverlayEntity, 512, 512));
            _inventoryOverlayEntity.Position = new Vector3(-3f, -0.5f, -8.0f);
            _inventoryOverlayEntity.Scale = new Vector3(3.0f, 5.0f, 1.0f);
            _inventoryOverlayEntity.RotateY(MathHelper.PiOver4 * 0.5f);

            var cameraEntity = world.EntitySystem.GetPendingEntityById("MainCamera");
            _inventoryOverlayEntity.SetParent(cameraEntity);

            _inventoryOverlayEntity.IsActive = false;

            _uiOverlay = new UiOverlay();
            _uiOverlay.AddElement(new HexagonBackground());
            _uiOverlay.AddElement(new LeftSideButton(LocalizedValue.Static("Pokemon"), new Vector2(26, 45), null));
            _uiOverlay.AddElement(new LeftSideButton(LocalizedValue.Static("Items"), new Vector2(26, 107), null));
            _uiOverlay.AddElement(new LeftSideButton(LocalizedValue.Static("Save"), new Vector2(26, 107), null));
            _uiOverlay.AddElement(new LeftSideButton(LocalizedValue.Static("Settings"), new Vector2(26, 169), null));
            _uiOverlay.AddElement(new LeftSideButton(LocalizedValue.Static("Quit"), new Vector2(26, 231), null));
            _uiOverlay.Show();
        }

        public void Update(GameTime gameTime)
        {
            if (GameInstance.GetService<InputSystem>().Keyboard.IsKeyDownOnce(Keys.P))
            {
                _inventoryOverlayEntity.IsActive = !_inventoryOverlayEntity.IsActive;
            }

            if (_inventoryOverlayEntity.IsActive) _uiOverlay.Update(gameTime);
        }

        public void DrawInventory()
        {
            if (!_inventoryOverlayEntity.IsActive) return;

            var old = _device.GetRenderTargets();
            _device.SetRenderTarget(_overlayComponent.DrawTarget);
            _device.Clear(ClearOptions.Target, Color.Transparent, 1.0f, 0);

            _uiOverlay.Draw(_spriteBatch);

            _device.SetRenderTargets(old);

        }
    }
}
