using Pokemon3D.Scripting.Types;
using Pokemon3D.Scripting;
using Pokemon3D.Scripting.Adapters;
using Pokemon3D.Screens;
using Pokemon3D.ScriptPipeline.Prototypes;
using Pokemon3D.Common.ScriptPipeline;

namespace Pokemon3D.ScriptPipeline.ApiClasses
{
    [ApiClass(ClassName = "World")]
    internal class World : ApiClass
    {
        public static SObject getEntity(ScriptProcessor processor, SObject[] parameters)
        {
            object[] netObjects;
            if (EnsureTypeContract(parameters, new[] { typeof(string) }, out netObjects))
            {
                var entity = new EntityWrapper {id = (string)netObjects[0]};

                return ScriptInAdapter.Translate(processor, entity);
            }

            return ScriptInAdapter.GetUndefined(processor);
        }

        public static SObject load(ScriptProcessor processor, SObject[] parameters)
        {
            object[] netObjects;
            if (EnsureTypeContract(parameters, new[] { typeof(string), typeof(Vector3Wrapper) }, out netObjects))
            {
                var screen = (OverworldScreen)GameProvider.GameInstance.GetService<ScreenManager>().CurrentScreen;

                var position = netObjects[1] as Vector3Wrapper;
                if (position != null)
                    screen.ActiveWorld.LoadMap(netObjects[0] as string, position.X, position.Y, position.Z);
            }

            return ScriptInAdapter.GetUndefined(processor);
        }
    }
}
