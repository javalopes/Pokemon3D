﻿using Pokemon3D.Scripting.Types;
using Pokemon3D.Scripting;
using Pokemon3D.Scripting.Adapters;
using Pokemon3D.Screens;
using Pokemon3D.ScriptPipeline.Prototypes;
using Pokemon3D.Common.ScriptPipeline;
using Pokemon3D.GameCore;

namespace Pokemon3D.ScriptPipeline.ApiClasses
{
    [ApiClass(ClassName = "World")]
    internal class World : ApiClass
    {
        // ReSharper disable InconsistentNaming
        public static SObject getEntity(ScriptProcessor processor, SObject[] parameters)
        // ReSharper restore InconsistentNaming
        {
            object[] netObjects;
            if (EnsureTypeContract(parameters, new[] { typeof(string) }, out netObjects))
            {
                var entity = new EntityWrapper {id = (string)netObjects[0]};

                return ScriptInAdapter.Translate(processor, entity);
            }

            return ScriptInAdapter.GetUndefined(processor);
        }

        // ReSharper disable InconsistentNaming
        public static SObject load(ScriptProcessor processor, SObject[] parameters)
        // ReSharper restore InconsistentNaming
        {
            object[] netObjects;
            if (EnsureTypeContract(parameters, new[] { typeof(string), typeof(Vector3Wrapper) }, out netObjects))
            {
                var screen = (OverworldIScreen)GameProvider.IGameInstance.GetService<ScreenManager>().CurrentIScreen;

                var position = netObjects[1] as Vector3Wrapper;
                if (position != null)
                    screen.ActiveWorld.LoadMap(netObjects[0] as string, position.X, position.Y, position.Z);
            }

            return ScriptInAdapter.GetUndefined(processor);
        }
    }
}
