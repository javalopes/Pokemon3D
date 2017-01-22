using System;
using System.Linq;

namespace Pokemon3D.Common.ScriptPipeline
{
    /// <summary>
    /// Ensures type contracts for script calls.
    /// </summary>
    public static class TypeContract
    {
        /// <summary>
        /// Ensures a type contract with a single typed argument.
        /// </summary>
        public static bool Ensure(object[] objects, Type typeContract, int optionalCount = 0)
        {
            return Ensure(objects, new[] { typeContract }, optionalCount);
        }

        /// <summary>
        /// Ensures a type contract with multiple typed arguments.
        /// </summary>
        public static bool Ensure(object[] objects, Type[] typeContract, int optionalCount = 0)
        {
            if (optionalCount > typeContract.Length)
                throw new ArgumentOutOfRangeException(nameof(optionalCount));
            if (objects.Length + optionalCount < typeContract.Length)
                return false;

            return !typeContract.Where((t, i) =>
            {
                if (t == null)
                    return false;

                if (objects.Length <= i)
                    return i < typeContract.Length - optionalCount;

                return objects[i] != null && objects[i].GetType() != t;
            }).Any();
        }

        /// <summary>
        /// Ensures a type contract with multiple typed argument, with multiple types per argument.
        /// </summary>
        public static bool Ensure(object[] objects, Type[][] typeContract, int optionalCount = 0)
        {
            if (optionalCount > typeContract.Length)
                throw new ArgumentOutOfRangeException(nameof(optionalCount));
            if (objects.Length + optionalCount < typeContract.Length)
                return false;

            return !typeContract.Where((types, i) =>
            {
                if (types == null || types.Length == 0)
                    return false;

                if (objects.Length <= i)
                    return i < typeContract.Length - optionalCount;

                if (objects[i] != null)
                {
                    var type = objects[i].GetType();
                    return types.Any(t => type == t);
                }

                return false;
            }).Any();
        }

        private static Type[] _numberTypeBuffer;

        /// <summary>
        /// Returns a type contract that returns true for Ensure methods for both numberic types <see cref="double"/> and <see cref="int"/>.
        /// </summary>
        public static Type[] Number => _numberTypeBuffer ?? (_numberTypeBuffer = new[] {typeof(double), typeof(int)});
    }
}
