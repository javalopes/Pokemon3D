using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.Scripting.Types;
using Pokemon3D.Scripting;
using Pokemon3D.Scripting.Adapters;

namespace Pokemon3D.ScriptPipeline.APIClasses
{
    [APIClass(ClassName = "world")]
    class World : APIClass
    {
        public static SObject getEntity(ScriptProcessor processor, SObject[] parameters)
        {
            object[] netObjects;
            if (EnsureTypeContract(parameters, new Type[] { typeof(string) }, out netObjects))
            {
                var entity = new EntityWrapper();
                entity.id = (string)netObjects[0];

                return ScriptInAdapter.Translate(processor, entity);
            }

            return ScriptInAdapter.GetUndefined(processor);
        }
    }
}
