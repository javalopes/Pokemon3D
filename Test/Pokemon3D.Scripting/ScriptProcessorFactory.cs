using Pokemon3D.Scripting;
using Pokemon3D.Scripting.Types;

namespace Test.Pokemon3D.Scripting
{
    /// <summary>
    /// A factory to create <see cref="ScriptProcessor"/> instances.
    /// </summary>
    internal static class ScriptProcessorFactory
    {
        /// <summary>
        /// Creates a new <see cref="ScriptProcessor"/>.
        /// </summary>
        internal static ScriptProcessor GetNew()
        {
            return new ScriptProcessor();
        }

        /// <summary>
        /// Creates a new <see cref="ScriptProcessor"/> and runs the provided source code.
        /// </summary>
        internal static ScriptProcessor GetRun(string source)
        {
            var processor = new ScriptProcessor();

            processor.Run(source);

            return processor;
        }
         
        internal static SObject Run(string source)
        {
            return GetNew().Run(source);
        }
    }
}
