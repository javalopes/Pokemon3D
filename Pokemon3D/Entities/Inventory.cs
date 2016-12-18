
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.Common.Input;
using Pokemon3D.Entities.System;
using Pokemon3D.Entities.System.Components;
using static GameProvider;

namespace Pokemon3D.Entities
{
    class Inventory
    {
        private readonly Entity _inventoryOverlay;

        public Inventory(World world)
        {
            _inventoryOverlay = world.EntitySystem.CreateEntity(true);
            _inventoryOverlay.AddComponent(new WorldUiOverlayEntityComponent(_inventoryOverlay, 512, 512));
            _inventoryOverlay.Position = new Vector3(-1.0f, 0.0f, -5.0f);

            _inventoryOverlay.IsActive = false;
        }

        public void Update()
        {
            if (GameInstance.GetService<InputSystem>().Keyboard.IsKeyDownOnce(Keys.P))
            {
                _inventoryOverlay.IsActive = !_inventoryOverlay.IsActive;
            }
        }
    }
}
