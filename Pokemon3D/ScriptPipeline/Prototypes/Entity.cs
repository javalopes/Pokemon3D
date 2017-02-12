using Pokemon3D.Common.ScriptPipeline;
using Pokemon3D.Entities.System;
using Pokemon3D.Screens;
using Pokemon3D.Scripting.Adapters;

namespace Pokemon3D.ScriptPipeline.Prototypes
{
    [ScriptPrototype(VariableName = "Entity")]
    internal class EntityWrapper
    {
        [ScriptVariable]
        // ReSharper disable InconsistentNaming
        public string id;
        // ReSharper restore InconsistentNaming

        [ScriptFunction(ScriptFunctionType.Standard, VariableName = "getComponent")]
        public static object GetComponent(object This, ScriptObjectLink objLink, object[] parameters)
        {
            if (TypeContract.Ensure(parameters, new[] { typeof(string) }))
            {
                var wrapper = (EntityWrapper)This;
                wrapper.GetEntity();

                var component = new EntityComponentWrapper
                {
                    parent = wrapper,
                    id = parameters[0] as string
                };

                return component;
            }

            return NetUndefined.Instance;
        }

        public Entity GetEntity()
        {
            // attempt to get world instance from world container screen:
            var screen = GameCore.IGameController.Instance.GetService<ScreenManager>().CurrentIScreen;
            var container = screen as IWorldContainer;
            var world = container?.ActiveWorld;
            return world?.EntitySystem.GetEntity(id);
        }
    }
}
