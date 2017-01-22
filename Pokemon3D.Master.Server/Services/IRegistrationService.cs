using System.Collections.Generic;
using Pokemon3D.DataModel.Multiplayer;

namespace Pokemon3D.Master.Server.Services
{
    public interface IRegistrationService
    {
        int Register(GameServerRegistrationModel instanceData);

        void Unregister(int id);

        IEnumerable<GameServerModel> GetRegisteredInstances();
    }
}