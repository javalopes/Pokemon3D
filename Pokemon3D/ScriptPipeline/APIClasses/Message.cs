using System;
using System.Threading;
using Pokemon3D.Scripting;
using Pokemon3D.Scripting.Adapters;
using Pokemon3D.Scripting.Types;
using Pokemon3D.Screens;
using Pokemon3D.Common.ScriptPipeline;
using Pokemon3D.UI;

namespace Pokemon3D.ScriptPipeline.ApiClasses
{
    [ApiClass(ClassName = "Message")]
    internal class Message : ApiClass
    {
        public static SObject show(ScriptProcessor processor, SObject[] parameters)
        {
            object[] netObjects;
            if (EnsureTypeContract(parameters, new[] { typeof(string) }, out netObjects))
            {
                var text = (string)netObjects[0];

                var screen = (OverworldScreen)GameProvider.GameInstance.GetService<ScreenManager>().CurrentScreen;
                var uiElement = new MessageOverworldUiElement(text);

                screen.AddOverlay(uiElement);
                uiElement.Show();

                var autoResetEvent = new AutoResetEvent(false);
                uiElement.Hidden += () => autoResetEvent.Set();

                BlockThreadUntilCondition(() => autoResetEvent.WaitOne());

                screen.RemoveOverlay(uiElement);
            }

            return ScriptInAdapter.GetUndefined(processor);
        }
    }
}
