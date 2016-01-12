using System.Collections.Generic;

namespace Pokemon3D.GameJolt
{
    // this part of the API class contains methods that create API call instances.

    public partial class API
    {
        /// <summary>
        /// Contains static methods to create valid API calls to the Game Jolt API.
        /// </summary>
        public static partial class Calls
        {
            /// <summary>
            /// Contains static methods to create valid API calls to the Game Jolt API accessing the game's data storage.
            /// </summary>
            public static class Storage
            {
                /// <summary>
                /// Creates an API call to fetch data from an entry in the game's global data storage.
                /// </summary>
                /// <param name="key">The key to return the data from.</param>
                public static APICall FetchGlobal(string key)
                {
                    var parameters = new Dictionary<string, string>();
                    parameters.Add("key", key);
                    return new APICall("data-store", parameters);
                }

                /// <summary>
                /// Creates an API call to fetch data from an entry in the user's data storage.
                /// </summary>
                /// <param name="key">The key to return the data from.</param>
                /// <param name="username">The username of the user.</param>
                /// <param name="token">The token of the user.</param>
                public static APICall FetchUser(string key, string username, string token)
                {
                    var parameters = new Dictionary<string, string>();
                    parameters.Add("key", key);
                    parameters.Add("username", username);
                    parameters.Add("user_token", token);
                    return new APICall("data-store", parameters);
                }

                /// <summary>
                /// Creates an API call to get all storage keys for this game.
                /// </summary>
                public static APICall GetKeys()
                {
                    return new APICall("data-store/get-keys");
                }

                /// <summary>
                /// Creates an API call to get all storage keys for this game that match the given pattern.
                /// </summary>
                public static APICall GetKeys(string pattern)
                {
                    var parameters = new Dictionary<string, string>();
                    parameters.Add("pattern", pattern);
                    return new APICall("data-store/get-keys", parameters);
                }
            }
        }
    }
}
