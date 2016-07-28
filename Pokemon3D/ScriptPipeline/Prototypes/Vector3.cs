using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.Scripting.Adapters;

namespace Pokemon3D.ScriptPipeline.Prototypes
{
    [ScriptPrototype(VariableName = "Vector3")]
    internal class Vector3Wrapper
    {
        [ScriptVariable(VariableName = "x")]
        public double X;

        [ScriptVariable(VariableName = "y")]
        public double Y;

        [ScriptVariable(VariableName = "z")]
        public double Z;

        [ScriptFunction(ScriptFunctionType.Constructor, VariableName = "constructor")]
        public static string Constructor = "function(x,y,z) { this.x = x; this.y = y; this.z = z; }";
    }
}
