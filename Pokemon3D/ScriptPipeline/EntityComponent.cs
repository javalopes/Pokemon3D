using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.Scripting.Adapters;

namespace Pokemon3D.ScriptPipeline
{
    [ScriptPrototype(VariableName = "entityComponent")]
    class EntityComponentWrapper
    {
        [ScriptVariable]
        public EntityWrapper parent;

        [ScriptVariable]
        public string id;

        [ScriptFunction]
        public static object setData(object This, object[] parameters)
        {
            var instance = (EntityComponentWrapper)This;
            var entity = instance.parent.GetEntity();
            var component = entity.GetComponent(instance.id);

            string dataKey = (string)parameters[0];
            string dataValue = (string)parameters[1];

            component.SetData(dataKey, dataValue);

            return dataValue;
        }

        [ScriptFunction]
        public static object getData(object This, object[] parameters)
        {
            var instance = (EntityComponentWrapper)This;
            var entity = instance.parent.GetEntity();
            var component = entity.GetComponent(instance.id);

            string dataKey = (string)parameters[0];

            return component.GetDataOrDefault(dataKey, "");
        }
    }
}
