using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.ScriptPipeline.Prototypes
{
    static class TypeContract
    {
        public static bool Ensure(object[] objects, Type[] typeContract)
        {
            if (objects.Length == typeContract.Length)
            {
                for (int i = 0; i < typeContract.Length; i++)
                {
                    if (objects[i] != null && objects[i].GetType() != typeContract[i])
                        return false;
                }

                return true;
            }

            return false;
        }
    }
}
