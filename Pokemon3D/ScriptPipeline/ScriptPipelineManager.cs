using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Pokemon3D.Common.Diagnostics;
using Pokemon3D.Entities;
using Pokemon3D.Scripting;
using Pokemon3D.Scripting.Adapters;
using Pokemon3D.Scripting.Types;
using Pokemon3D.ScriptPipeline.APIClasses;
using static Pokemon3D.GameCore.GameProvider;
using Pokemon3D.UI;

namespace Pokemon3D.ScriptPipeline
{
    static class ScriptPipelineManager
    {
        public static int ActiveProcessorCount { get; private set; }

        public static int ThreadBlockingOperationCount { get; set; }

        private static List<SObject> _prototypeBuffer;
        private static Dictionary<string, MethodInfo[]> _apiClasses;

        private static void InitializePrototypeBuffer()
        {
            // load all defined prototypes once to store them in a buffer.
            // these prototypes get added to all newly created ScriptProcessors.

            _prototypeBuffer = new List<SObject>();
            var processor = new ScriptProcessor();

            foreach (Type t in typeof(ScriptPipelineManager).Assembly.GetTypes().Where(t => t.GetCustomAttributes(typeof(ScriptPrototypeAttribute), true).Length > 0))
                _prototypeBuffer.Add(ScriptInAdapter.Translate(processor, t));
        }

        private static void InitializeAPIClasses()
        {
            // get all types derived from APIClass that have an APIClass attribute.
            // get their public static methods and add them grouped by their type's name to the dictionary.

            _apiClasses = new Dictionary<string, MethodInfo[]>();
            foreach (Type t in typeof(ScriptPipelineManager).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(APIClass)) &&
                                                                                            t.GetCustomAttributes(typeof(APIClassAttribute), true).Length > 0))
            {
                var attr = (APIClassAttribute)t.GetCustomAttribute(typeof(APIClassAttribute));
                _apiClasses.Add(attr.ClassName, t.GetMethods(BindingFlags.Public | BindingFlags.Static));
            }
        }

        private static ScriptProcessor CreateProcessor()
        {
            if (_prototypeBuffer == null)
                InitializePrototypeBuffer();

            var processor = new ScriptProcessor(_prototypeBuffer);

            ScriptContextManipulator.SetCallbackExecuteMethod(processor, ExecuteMethod);

            return processor;
        }

        private static SObject ExecuteMethod(ScriptProcessor processor, string className, string methodName, SObject[] parameters)
        {
            if (_apiClasses == null)
                InitializeAPIClasses();

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
            ActiveProcessorCount++;
            ThreadPool.QueueUserWorkItem(o =>
            {
                try
                {
                    //todo: HÜLFE!
                    var gamemode = GameInstance.GetService<GameModeManager>().ActiveGameMode;
                    string source = Encoding.UTF8.GetString(gamemode.FileLoader.GetFile(gamemode.GetScriptFilePath(scriptFile), false).Data);

                    var processor = CreateProcessor();
                    var result = processor.Run(source);

                    if (ScriptContextManipulator.ThrownRuntimeError(processor))
                    {
                        var exObj = ScriptOutAdapter.Translate(result);
                        if (exObj is ScriptRuntimeException)
                            throw (ScriptRuntimeException)exObj;
                    }
                }
                catch (ArgumentNullException)
                {
                    var message = "Failed to run script \"" + scriptFile + "\"";
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
