using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.Scripting.Adapters;

namespace Pokemon3D.ScriptPipeline.Prototypes
{
    [ScriptPrototype(VariableName = "Vector2")]
    class Vector2Wrapper
    {
        [ScriptVariable]
        public double x;

        [ScriptVariable]
        public double y;

        [ScriptFunction(ScriptFunctionType.Constructor)]
        public static string constructor = "function(x,y) { this.x = x; this.y = y; }";
    }
}
