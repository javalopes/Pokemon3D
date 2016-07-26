using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.Scripting.Adapters;

namespace Pokemon3D.ScriptPipeline.Prototypes
{
    [ScriptPrototype(VariableName = "Vector3")]
    class Vector3Wrapper
    {
        [ScriptVariable]
        public double x;

        [ScriptVariable]
        public double y;

        [ScriptVariable]
        public double z;

        [ScriptFunction(ScriptFunctionType.Constructor)]
        public static string constructor = "function(x,y,z) { this.x = x; this.y = y; this.z = z; }";
    }
}
