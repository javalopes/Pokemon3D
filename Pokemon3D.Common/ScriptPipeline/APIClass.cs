using Pokemon3D.Scripting.Adapters;
using Pokemon3D.Scripting.Types;
using System;
using System.Threading;

namespace Pokemon3D.Common.ScriptPipeline
{
    public abstract class ApiClass
    {
        /// <summary>
        /// Ensures that the parameters for an API method fulfill the type contract. It also converts all script objects to .Net objects.
        /// </summary>
        protected static bool EnsureTypeContract(SObject[] parameters, Type[] typeContract, out object[] netObjects)
        {
            if (parameters.Length >= typeContract.Length)
            {
                netObjects = new object[parameters.Length];
                var i = 0;

                while (i < parameters.Length)
                {
                    netObjects[i] = ScriptOutAdapter.Translate(parameters[i]);

                    if (i < typeContract.Length && typeContract[i] != netObjects[i].GetType())
                        return false;

                    i++;
                }

                return true;
            }
            else
            {
                netObjects = null;
                return false;
            }
        }

        /// <summary>
        /// Blocks the currently running script until a condition is met.
        /// </summary>
        protected static void BlockThreadUntilCondition(Func<bool> condition)
        {
            SpinWait.SpinUntil(condition);
        }
    }
}
