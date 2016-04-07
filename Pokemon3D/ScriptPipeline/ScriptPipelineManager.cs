using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.Scripting;
using Pokemon3D.Scripting.Adapters;
using Pokemon3D.Scripting.Types;
using System.Reflection;

namespace Pokemon3D.ScriptPipeline
{
    class ScriptPipelineManager
    {
        private static List<SObject> _prototypeBuffer;

        private static void Initialize()
        {
            _prototypeBuffer = new List<SObject>();
            var processor = new ScriptProcessor();

            foreach (Type t in Assembly.GetCallingAssembly().GetTypes().Where(t => t.GetCustomAttributes(typeof(ScriptPrototypeAttribute), true).Length > 0))
                _prototypeBuffer.Add(ScriptInAdapter.Translate(processor, t));
        }

        public static ScriptProcessor CreateProcessor()
        {
            if (_prototypeBuffer == null)
                Initialize();
            return new ScriptProcessor(_prototypeBuffer);
        }
    }
}
