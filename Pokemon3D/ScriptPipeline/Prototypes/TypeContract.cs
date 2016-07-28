using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.ScriptPipeline.Prototypes
{
    internal static class TypeContract
    {
        public static bool Ensure(object[] objects, Type[] typeContract)
        {
            if (objects.Length != typeContract.Length) return false;
            return !typeContract.Where((t, i) => objects[i] != null && objects[i].GetType() != t).Any();
        }
    }
}
