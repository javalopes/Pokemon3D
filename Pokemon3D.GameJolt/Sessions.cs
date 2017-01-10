using System.Collections.Generic;

namespace Pokemon3D.GameJolt.API_Calls
{
    public partial class Api
    {
        public static partial class Calls
        {
            /// <summary>
            /// Contains static methods to create valid API calls to the Game Jolt API to utilize sessions.
            /// </summary>
            public static class Sessions
            {
                /// <summary>
                /// Creates an API call that opens a session.
                /// </summary>
                public static ApiCall Open(string username, string token)
                {
                    var parameters = new Dictionary<string, string>();
                    parameters.Add("username", username);
                    parameters.Add("user_token", token);
                    return new ApiCall("sessions/open", parameters);
                }

                /// <summary>
                /// Creates an API call that pings a session.
                /// </summary>
                public static ApiCall Ping(string username, string token)
                {
                    var parameters = new Dictionary<string, string>();
                    parameters.Add("username", username);
                    parameters.Add("user_token", token);
                    return new ApiCall("sessions/ping", parameters);
                }

                /// <summary>
                /// Creates an API call that closes a session.
                /// </summary>
                public static ApiCall Close(string username, string token)
                {
                    var parameters = new Dictionary<string, string>();
                    parameters.Add("username", username);
                    parameters.Add("user_token", token);
                    return new ApiCall("sessions/close", parameters);
                }
            }
        }
    }
}
