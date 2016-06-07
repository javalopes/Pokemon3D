using System;
using Pokemon3D.Screens.Overworld;
using Pokemon3D.Scripting;
using Pokemon3D.Scripting.Adapters;
using Pokemon3D.Scripting.Types;

namespace Pokemon3D.ScriptPipeline.APIClasses
{
    [APIClass(ClassName = "message")]
    class Message : APIClass
    {
        public static SObject show(ScriptProcessor processor, SObject[] parameters)
        {
            object[] netObjects;
            if (EnsureTypeContract(parameters, new Type[] { typeof(string) }, out netObjects))
            {
                var text = (string)netObjects[0];

                OverworldScreen screen = (OverworldScreen)GameCore.GameProvider.GameInstance.ScreenManager.CurrentScreen;
                MessageOverworldUIElement uiElement = new MessageOverworldUIElement(text);

                screen.AddUiElement(uiElement);
                uiElement.IsActive = true;

                BlockThreadUntilCondition(() => !uiElement.IsActive);

                screen.RemoveUiElement(uiElement);
            }

            return ScriptInAdapter.GetUndefined(processor);
        }
    }
}
