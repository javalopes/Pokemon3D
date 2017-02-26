using Microsoft.Xna.Framework;
using Pokemon3D.Rendering;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.Entities
{
    internal class Weather
    {
        private readonly Light _mainLight;

        public Weather()
        {
            var renderer = GameInstance.GetService<ISceneRenderer>();

            _mainLight = renderer.CreateDirectionalLight(new Vector3(-1.5f, -1.0f, -0.5f));
            _mainLight.AmbientIntensity = 0.5f;
            _mainLight.DiffuseIntensity = 0.8f;
            renderer.AmbientLight = new Vector4(0.7f, 0.5f, 0.5f, 1.0f);
        }

        public void Clear()
        {
            GameInstance.GetService<ISceneRenderer>().RemoveLight(_mainLight);
        }
    }
}
