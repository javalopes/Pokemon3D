using System;
using Lidgren.Network;

namespace Pokemon3D.Server.Management
{
    public class Player
    {
        public Guid UniqueIdentifier { get; }
        public NetConnection Connection { get; }

        public string Name { get; }

        public Player(string name, NetConnection connection)
        {
            Connection = connection;
            UniqueIdentifier = Guid.NewGuid();
            Name = name;
        }

        public override string ToString()
        {
            return $"[{UniqueIdentifier}] '{Name}'";
        }
    }
}
