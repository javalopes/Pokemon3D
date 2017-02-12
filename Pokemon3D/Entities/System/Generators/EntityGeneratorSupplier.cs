using System.Collections.Generic;

namespace Pokemon3D.Entities.System.Generators
{
    internal class EntityGeneratorSupplier
    {
        private readonly Dictionary<string, IEntityGenerator> _generators;

        public EntityGeneratorSupplier()
        {
            _generators = new Dictionary<string, IEntityGenerator>()
                {
                    { "simple", new SimpleIEntityGenerator() },
                    { "texturedcube", new TexturedCubeIEntityGenerator() }
                };
        }

        public IEntityGenerator GetGenerator(string identifier)
        { 
            if (string.IsNullOrWhiteSpace(identifier))
            {
                return _generators["simple"];
            }
            else
            {
                identifier = identifier.ToLowerInvariant();
                if (_generators.ContainsKey(identifier))
                {
                    return _generators[identifier];
                }
                else
                {
                    return _generators["simple"];
                }
            }
        }
    }
}
