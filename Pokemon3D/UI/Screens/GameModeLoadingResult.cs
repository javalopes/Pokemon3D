using Pokemon3D.GameModes.Maps;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Compositor;

namespace Pokemon3D.UI.Screens
{
    class GameModeLoadingResult
    {
        public SceneRenderer SceneRenderer { get; set; }
        public Scene Scene { get; set; }
        public Player Player { get; set; }
        public Map Map { get; set; }
    }
}
