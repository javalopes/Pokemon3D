using System.Collections.Generic;
using Pokemon3D.Master.Server.DataContracts;

namespace Pokemon3D.Master.Server.Services
{
    public interface IRegistrationService
    {
        int Register(GameServerRegistrationData instanceData);

        void Unregister(int id);

        IEnumerable<GameServerData> GetRegisteredInstances();
    }
}