using System.Collections.Generic;
using System.Web.Http;
using Pokemon3D.Master.Server.DataContracts;
using Pokemon3D.Master.Server.Services;

namespace Pokemon3D.Master.Server.Controllers
{
    public class GameServerController : ApiController
    {
        private readonly IRegistrationService _service;

        public GameServerController(IRegistrationService service)
        {
            _service = service;
        }

        [Route("api/gameserver/register")]
        [HttpPost]
        public IHttpActionResult Register(GameServerRegistrationData data)
        {
            var id = _service.Register(data);

            return Ok(id);
        }

        [Route("api/gameserver/unregister")]
        [HttpPost]
        public IHttpActionResult UnRegister(int id)
        {
            _service.Unregister(id);

            return Ok();
        }

        [Route("api/gameserver/instances")]
        [HttpGet]
        public IEnumerable<GameServerData> GetInstances()
        {
            return _service.GetRegisteredInstances();
        }
    }
}
