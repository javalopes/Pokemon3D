using System.Collections.Generic;

namespace Pokemon3D.GameJolt
{
    public partial class API
    {
        public static partial class Calls
        {
            /// <summary>
            /// Contains static methods to create valid API calls to the Game Jolt API accessing user functionalities.
            /// </summary>
            public static class Users
            {
                /// <summary>
                /// Creates an API call that authorizes a user with a token. The result indicates wether this was successful.
                /// </summary>
                public static APICall Authorize(string username, string token)
                {
                    var parameters = new Dictionary<string, string>();
                    parameters.Add("username", username);
                    parameters.Add("user_token", token);
                    return new APICall("users/auth", parameters);
                }

                /// <summary>
                /// Creates an API call that fetches user data from the server based on the passed in username.
                /// </summary>
                public static APICall FetchData(string username)
                {
                    var parameters = new Dictionary<string, string>();
                    parameters.Add("username", username);
                    return new APICall("users", parameters);
                }

                /// <summary>
                /// Creates an API call that fetches user data from the server based on the passed in Game Jolt user id.
                /// </summary>
                public static APICall FetchDataById(string userId)
                {
                    var parameters = new Dictionary<string, string>();
                    parameters.Add("user_id", userId);
                    return new APICall("users", parameters);
                }
            }
        }
    }
}
