using System.Collections.Generic;

namespace Pokemon3D.GameJolt
{
    public partial class API
    {
        public static partial class Calls
        {
            /// <summary>
            /// Contains static methods to create valid API calls to the Game Jolt API to the server's time data.
            /// </summary>
            public static class Time
            {
                /// <summary>
                /// Creates an API call that returns time information of the Game Jolt API server.
                /// </summary>
                public static APICall GetTime()
                {
                    return new APICall("get-time");
                }
            }
        }
    }
}
