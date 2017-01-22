using System.Collections.Generic;

namespace Pokemon3D.GameJolt
{
    public partial class Api
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
                public static ApiCall Authorize(string username, string token)
                {
                    var parameters = new Dictionary<string, string> {{"username", username}, {"user_token", token}};
                    return new ApiCall("users/auth", parameters);
                }

                /// <summary>
                /// Creates an API call that fetches user data from the server based on the passed in username.
                /// </summary>
                public static ApiCall FetchData(string username)
                {
                    var parameters = new Dictionary<string, string> {{"username", username}};
                    return new ApiCall("users", parameters);
                }

                /// <summary>
                /// Creates an API call that fetches user data from the server based on the passed in Game Jolt user id.
                /// </summary>
                public static ApiCall FetchDataById(string userId)
                {
                    var parameters = new Dictionary<string, string> {{"user_id", userId}};
                    return new ApiCall("users", parameters);
                }
            }
        }
    }
}
