using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.Scripting.Adapters;

namespace Pokemon3D.ScriptPipeline.Prototypes
{
    [ScriptPrototype(VariableName = "Vector2")]
    internal class Vector2Wrapper
    {
        [ScriptVariable(VariableName = "x")]
        public double X;

        [ScriptVariable(VariableName = "y")]
        public double Y;

        [ScriptFunction(ScriptFunctionType.Constructor, VariableName = "constructor")]
        public static string Constructor = "function(x,y) { this.x = x; this.y = y; }";
    }
}
