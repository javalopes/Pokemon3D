using Pokemon3D.Common.ScriptPipeline;
using Pokemon3D.Scripting.Adapters;

namespace Pokemon3D.ScriptPipeline.Prototypes
{
    [ScriptPrototype(VariableName = "EntityComponent")]
    internal class EntityComponentWrapper
    {
        [ScriptVariable]
        // ReSharper disable InconsistentNaming
        public EntityWrapper parent;
        // ReSharper restore InconsistentNaming

        [ScriptVariable]
        // ReSharper disable InconsistentNaming
        public string id;
        // ReSharper restore InconsistentNaming

        [ScriptFunction(ScriptFunctionType.Standard, VariableName = "setData")]
        public static object SetData(object This, ScriptObjectLink objLink, object[] parameters)
        {
            if (TypeContract.Ensure(parameters, new[] { typeof(string), typeof(string) }))
            {
                var instance = (EntityComponentWrapper)This;
                var entity = instance.parent.GetEntity();
                var component = entity.GetComponent(instance.id);

                var dataKey = (string)parameters[0];
                var dataValue = (string)parameters[1];

                component.SetData(dataKey, dataValue);

                return dataValue;
            }

            return NetUndefined.Instance;
        }

        [ScriptFunction(ScriptFunctionType.Standard, VariableName = "getData")]
        public static object GetData(object This, ScriptObjectLink objLink, object[] parameters)
        {
            if (TypeContract.Ensure(parameters, typeof(string)))
            {
                var instance = (EntityComponentWrapper)This;
                var entity = instance.parent.GetEntity();
                var component = entity.GetComponent(instance.id);

                var dataKey = (string)parameters[0];

                return component.GetDataOrDefault(dataKey, "");
            }

            return NetUndefined.Instance;
        }
    }
}
