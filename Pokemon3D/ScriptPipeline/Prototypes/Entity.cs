using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.Screens.Overworld;
using Pokemon3D.Scripting.Adapters;
using Pokemon3D.Entities.System;
using Pokemon3D.Screens;

namespace Pokemon3D.ScriptPipeline.Prototypes
{
    [ScriptPrototype(VariableName = "entity")]
    internal class EntityWrapper
    {
        [ScriptVariable]
        public string id;

        [ScriptFunction(ScriptFunctionType.Standard, VariableName = "getComponent")]
        public static object GetComponent(object This, object[] parameters)
        {
            var wrapper = (EntityWrapper)This;
            var entity = wrapper.GetEntity();

            var component = new EntityComponentWrapper
            {
                parent = wrapper,
                id = parameters[0] as string
            };

            return component;
        }

        public Entity GetEntity()
        {
            // attempt to get world instance from world container screen:
            var screen = GameCore.GameController.Instance.GetService<ScreenManager>().CurrentScreen;
            var container = screen as WorldContainer;
            var world = container?.ActiveWorld;
            return world?.EntitySystem.GetEntity(id);
        }
    }
}
