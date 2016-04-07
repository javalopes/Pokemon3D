using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.Scripting.Adapters;

namespace Pokemon3D.ScriptPipeline
{
    [ScriptPrototype(VariableName = "Vector2")]
    class Vector2Wrapper
    {
        [ScriptVariable]
        public double X;

        [ScriptVariable]
        public double Y;
    }
}
