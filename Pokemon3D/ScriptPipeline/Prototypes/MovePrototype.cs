using Pokemon3D.Common.ScriptPipeline;
using Pokemon3D.DataModel.GameMode.Battle;
using Pokemon3D.GameModes;
using Pokemon3D.Scripting.Adapters;
using static Pokemon3D.GameProvider;

namespace Pokemon3D.ScriptPipeline.Prototypes
{
    [ScriptPrototype(VariableName = "move")]
    internal class MovePrototype
    {
        [ScriptVariable(VariableName = "id")]
        public string id;

        [ScriptVariable(VariableName = "PP")]
        public int PP;

        [ScriptVariable(VariableName = "maxPP")]
        public int maxPP;

        private static MoveModel GetMoveModel(object This)
        {
            var prototype = This as MovePrototype;
            var gameMode = GameInstance.GetService<GameModeManager>().ActiveGameMode;
            return gameMode.GetMoveModel(prototype.id);
        }

        [ScriptFunction(ScriptFunctionType.Constructor, VariableName = "constructor")]
        public static object Constructor(object This, ScriptObjectLink objLink, object[] parameters)
        {
            if (TypeContract.Ensure(parameters, new[] { typeof(string), typeof(int), typeof(int) }, 1))
            {
                var helper = new ParameterHelper(parameters);

                string id = helper.Pop<string>();
                objLink.SetMember("id", id);

                var moveModel = GetMoveModel(This);

                objLink.SetMember("PP", helper.Pop(moveModel.PP));
                objLink.SetMember("maxPP", helper.Pop(moveModel.PP));
            }

            return NetUndefined.Instance;
        }

        [ScriptFunction(ScriptFunctionType.Getter, VariableName = "name")]
        public static object GetName(object This, ScriptObjectLink objLink, object[] parameters)
        {
            return GetMoveModel(This).Name;
        }

        [ScriptFunction(ScriptFunctionType.Getter, VariableName = "description")]
        public static object GetDescription(object This, ScriptObjectLink objLink, object[] parameters)
        {
            return GetMoveModel(This).Description;
        }

        [ScriptFunction(ScriptFunctionType.Getter, VariableName = "types")]
        public static object GetTypes(object This, ScriptObjectLink objLink, object[] parameters)
        {
            return GetMoveModel(This).Types;
        }
    }
}
