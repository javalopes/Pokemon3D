using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.GameModes;
using Pokemon3D.GameModes.Monsters;
using Pokemon3D.Scripting.Adapters;
using static Pokemon3D.GameProvider;

namespace Pokemon3D.ScriptPipeline.Prototypes
{
    [ScriptPrototype(VariableName = "Pokemon")]
    internal class PokemonPrototype
    {
        [Reference]
        public Pokemon pokemonRef;

        [ScriptFunction(ScriptFunctionType.Constructor, VariableName = "constructor")]
        public static object Constructor(object This, ScriptObjectLink objLink, object[] parameters)
        {
            objLink.SetReference(nameof(pokemonRef), GameInstance.LoadedSave.PartyPokemon[0]);

            return NetUndefined.Instance;
        }

        [ScriptFunction(ScriptFunctionType.Standard, VariableName = "someFunction")]
        public static object SomeFunction(object This, ScriptObjectLink objLink, object[] parameters)
        {
            var test = objLink.GetReference(nameof(pokemonRef));
            return NetUndefined.Instance;
        }
    }
}
