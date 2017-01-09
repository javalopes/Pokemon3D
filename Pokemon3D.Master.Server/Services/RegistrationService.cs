using System.Collections.Generic;

namespace Pokemon3D.Master.Server.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly List<InstanceData> _instances = new List<InstanceData>();

        public void Register(InstanceData instanceData)
        {
            _instances.Add(instanceData);
        }

        public void Unregister(InstanceData instanceData)
        {
            _instances.Add(instanceData);
        }

        public IEnumerable<InstanceData> GetRegisteredInstances()
        {
            return _instances;
        }
    }
}