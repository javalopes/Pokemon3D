using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.GameJolt
{
    public partial class API
    {
        private const string HOST = "api.gamejolt.com/api/game";
        private const string VERSION = "v1_1";

        private const string FORMAT_CALL_URL = "http://{0}/{1}/batch/?game_id={2}&format={3}";

        private static string ResponseFormatToString(ResponseFormat format)
        {
            return format.ToString().ToLowerInvariant();
        }
    }
}
