using System.Collections.Generic;
using Pokemon3D.DataModel.Multiplayer;
namespace Pokemon3D.Master.Server.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly List<GameServerModel> _instances = new List<GameServerModel>();
        private int _nextId;

        public int Register(GameServerRegistrationModel instanceData)
        {
            var current = _nextId;
            _nextId++;
            _instances.Add(new GameServerModel
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

        public IEnumerable<GameServerModel> GetRegisteredInstances()
        {
            return _instances;
        }
    }
}