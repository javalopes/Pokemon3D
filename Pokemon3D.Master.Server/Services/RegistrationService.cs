using System.Collections.Generic;
using Pokemon3D.Master.Server.DataContracts;

namespace Pokemon3D.Master.Server.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly List<GameServerData> _instances = new List<GameServerData>();
        private int _nextId = 0;

        public int Register(GameServerRegistrationData instanceData)
        {
            var current = _nextId;
            _nextId++;
            _instances.Add(new GameServerData
            {
                Id = current,
                IpAddress = instanceData.IpAddress,
                Name = instanceData.Name
            });
            return current;
        }

        public void Unregister(int id)
        {
            _instances.RemoveAll(i => i.Id == id);
        }

        public IEnumerable<GameServerData> GetRegisteredInstances()
        {
            return _instances;
        }
    }
}