using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.Common.Diagnostics;
using Pokemon3D.Common.ScriptPipeline;
using Pokemon3D.GameModes;
using Pokemon3D.Scripting;
using Pokemon3D.Scripting.Adapters;
using Pokemon3D.Scripting.Types;
using Pokemon3D.UI;
using static GameProvider;

namespace Pokemon3D.ScriptPipeline
{
    internal static class ScriptPipelineManager
    {
        public static int ActiveProcessorCount { get; private set; }

        public static int ThreadBlockingOperationCount { get; set; }

        private static List<SObject> _prototypeBuffer;
        private static Dictionary<string, MethodInfo[]> _apiClasses;

        private static Assembly[] GetSourceAssemblies()
        {
            return new[]
            {
                typeof(ScriptPipelineManager).Assembly,
                AssemblyReference.Get()
            };
        }

        private static void InitializePrototypeBuffer()
        {
            // load all defined prototypes once to store them in a buffer.
            // these prototypes get added to all newly created ScriptProcessors.

            _prototypeBuffer = new List<SObject>();
            var processor = new ScriptProcessor();

            foreach (var assembly in GetSourceAssemblies())
            {
                foreach (var t in assembly.GetTypes().Where(t => t.GetCustomAttributes(typeof(ScriptPrototypeAttribute), true).Length > 0))
                    _prototypeBuffer.Add(ScriptInAdapter.Translate(processor, t));
            }
        }

        private static void InitializeApiClasses()
        {
            // get all types derived from APIClass that have an ApiClass attribute.
            // get their public static methods and add them grouped by their type's name to the dictionary.

            _apiClasses = new Dictionary<string, MethodInfo[]>();

            foreach (var assembly in GetSourceAssemblies())
            {
                foreach (var t in assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(ApiClass)) &&
                                                                 t.GetCustomAttributes(typeof(ApiClassAttribute), true).Length > 0))
                {
                    var attr = (ApiClassAttribute)t.GetCustomAttribute(typeof(ApiClassAttribute));
                    _apiClasses.Add(attr.ClassName, t.GetMethods(BindingFlags.Public | BindingFlags.Static));
                }
            }
        }

        private static ScriptProcessor CreateProcessor()
        {
            var processor = new ScriptProcessor(_prototypeBuffer);

            ScriptContextManipulator.SetCallbackExecuteMethod(processor, ExecuteMethod);

            return processor;
        }

        private static SObject ExecuteMethod(ScriptProcessor processor, string className, string methodName, SObject[] parameters)
        {
            if (_apiClasses.ContainsKey(className))
            {
                var method = _apiClasses[className].FirstOrDefault(m => m.Name == methodName);

                if (method != null)
                {
                    var result = method.Invoke(null, new object[] { processor, parameters });
                    return (SObject)result;
                }
            }

            // fallback, in case no class/method was found:
            return ScriptInAdapter.GetUndefined(processor);
        }

        public static void RunScript(string scriptFile)
        {
            if (_apiClasses == null)
                InitializeApiClasses();
            if (_prototypeBuffer == null)
                InitializePrototypeBuffer();

            ActiveProcessorCount++;
            Task.Run(() =>
            {
                try
                {
                    var gamemode = GameInstance.GetService<GameModeManager>().ActiveGameMode;
                    var source = Encoding.UTF8.GetString(gamemode.FileLoader.GetFile(gamemode.GetScriptFilePath(scriptFile), false).Data);

                    var processor = CreateProcessor();
                    var result = processor.Run(source);

                    if (ScriptContextManipulator.ThrownRuntimeError(processor))
                    {
                        var exObj = ScriptOutAdapter.Translate(result);

                        var runtimeException = exObj as ScriptRuntimeException;
                        if (runtimeException != null)
                            throw runtimeException;
                    }
                }
                catch (ArgumentNullException)
                {
                    var message = $"Failed to run script \"{scriptFile}\"";
                    GameLogger.Instance.Log(MessageType.Error, message);
                    GameInstance.GetService<NotificationBar>().PushNotification(NotificationKind.Error, message);
                }
                catch (ScriptRuntimeException ex)
                {
                    var message = $"Script execution failed at runtime. {ex.Type} ({scriptFile}, L{ex.Line}): {ex.Message}";
                    GameLogger.Instance.Log(MessageType.Error, message);
                    GameInstance.GetService<NotificationBar>().PushNotification(NotificationKind.Error, message);
                }

                ActiveProcessorCount--;
            });
        }
    }
}
