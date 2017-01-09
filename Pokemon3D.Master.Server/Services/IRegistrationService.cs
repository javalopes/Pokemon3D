using System.Collections;
using System.Collections.Generic;

namespace Pokemon3D.Master.Server.Services
{
    public interface IRegistrationService
    {
        void Register(InstanceData instanceData);

        void Unregister(InstanceData instanceData);

        IEnumerable<InstanceData> GetRegisteredInstances();
    }
}