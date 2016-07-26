using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.Screens.Overworld;
using Pokemon3D.Scripting.Adapters;
using Pokemon3D.Entities.System;
using Pokemon3D.Screens;

namespace Pokemon3D.ScriptPipeline
{
    [ScriptPrototype(VariableName = "entity")]
    class EntityWrapper
    {
        [ScriptVariable]
        public string id;

        [ScriptFunction(ScriptFunctionType.Standard)]
        public static object getComponent(object This, object[] parameters)
        {
            var wrapper = (EntityWrapper)This;
            var entity = wrapper.GetEntity();

            var component = new EntityComponentWrapper();
            component.parent = wrapper;
            component.id = (string)parameters[0];

            return component;
        }

        public Entity GetEntity()
        {
            // attempt to get world instance from world container screen:
            var screen = GameCore.GameController.Instance.GetService<ScreenManager>().CurrentScreen;
            if (screen is WorldContainer)
            {
                var world = ((WorldContainer)screen).ActiveWorld;
                return world.EntitySystem.GetEntity(id);
            }
            else
            {
                return null;
            }
        }
    }
}
