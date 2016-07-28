using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.Scripting.Adapters;
using Pokemon3D.ScriptPipeline.Prototypes;

namespace Pokemon3D.ScriptPipeline
{
    [ScriptPrototype(VariableName = "entityComponent")]
    internal class EntityComponentWrapper
    {
        [ScriptVariable]
        public EntityWrapper parent;

        [ScriptVariable]
        public string id;

        [ScriptFunction(ScriptFunctionType.Standard, VariableName = "setData")]
        public static object SetData(object This, object[] parameters)
        {
            var instance = (EntityComponentWrapper)This;
            var entity = instance.parent.GetEntity();
            var component = entity.GetComponent(instance.id);

            var dataKey = (string)parameters[0];
            var dataValue = (string)parameters[1];

            component.SetData(dataKey, dataValue);

            return dataValue;
        }

        [ScriptFunction(ScriptFunctionType.Standard, VariableName = "getData")]
        public static object GetData(object This, object[] parameters)
        {
            var instance = (EntityComponentWrapper)This;
            var entity = instance.parent.GetEntity();
            var component = entity.GetComponent(instance.id);

            var dataKey = (string)parameters[0];

            return component.GetDataOrDefault(dataKey, "");
        }
    }
}
