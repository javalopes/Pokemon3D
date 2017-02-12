using System;

namespace Pokemon3D.Server.Management
{
    public class Player
    {
        public Guid UniqueIdentifier { get; }

        public string Name { get; }

        public Player(string name)
        {
            UniqueIdentifier = Guid.NewGuid();
            Name = name;
        }

        public override string ToString()
        {
            return $"[{UniqueIdentifier}] '{Name}'";
        }
    }
}
