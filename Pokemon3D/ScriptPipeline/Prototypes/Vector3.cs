using Pokemon3D.Common.ScriptPipeline;
using Pokemon3D.Scripting.Adapters;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

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
        public static object Constructor(object This, ScriptObjectLink objLink, object[] parameters)
        {
            if (TypeContract.Ensure(parameters, new[] { TypeContract.Number, TypeContract.Number, TypeContract.Number }))
            {
                var x = (double)parameters[0];
                var y = (double)parameters[1];
                var z = (double)parameters[2];

                objLink.SetMember("x", x);
                objLink.SetMember("y", y);
                objLink.SetMember("z", z);
            }

            return NetUndefined.Instance;
        }
    }
}
