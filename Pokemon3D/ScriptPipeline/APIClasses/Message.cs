using System;
using Pokemon3D.Screens.Overworld;
using Pokemon3D.Scripting;
using Pokemon3D.Scripting.Adapters;
using Pokemon3D.Scripting.Types;
using Pokemon3D.Screens;
using Pokemon3D.Common.ScriptPipeline;

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

                var screen = (OverworldScreen)GameCore.GameProvider.GameInstance.GetService<ScreenManager>().CurrentScreen;
                var uiElement = new MessageOverworldUIElement(text);

                screen.AddUiElement(uiElement);
                uiElement.IsActive = true;

                BlockThreadUntilCondition(() => !uiElement.IsActive);

                screen.RemoveUiElement(uiElement);
            }

            return ScriptInAdapter.GetUndefined(processor);
        }
    }
}
