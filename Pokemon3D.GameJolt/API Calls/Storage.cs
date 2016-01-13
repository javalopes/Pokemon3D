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
                public static APICall GetKeysGlobal()
                {
                    return new APICall("data-store/get-keys");
                }

                /// <summary>
                /// Creates an API call to get all storage keys for this game that match the given pattern.
                /// </summary>
                public static APICall GetKeysGlobal(string pattern)
                {
                    var parameters = new Dictionary<string, string>();
                    parameters.Add("pattern", pattern);
                    return new APICall("data-store/get-keys", parameters);
                }

                /// <summary>
                /// Creates an API call to get all storage keys for this user.
                /// </summary>
                public static APICall GetKeysUser(string username, string token)
                {
                    var parameters = new Dictionary<string, string>();
                    parameters.Add("username", username);
                    parameters.Add("user_token", token);
                    return new APICall("data-store/get-keys", parameters);
                }

                /// <summary>
                /// Creates an API call to get all storage keys for this user that match the given pattern.
                /// </summary>
                public static APICall GetKeysUser(string pattern, string username, string token)
                {
                    var parameters = new Dictionary<string, string>();
                    parameters.Add("pattern", pattern);
                    parameters.Add("username", username);
                    parameters.Add("user_token", token);
                    return new APICall("data-store/get-keys", parameters);
                }

                /// <summary>
                /// Creates an API call that updates a value in the global data storage.
                /// </summary>
                public static APICall UpdateGlobal(string key, StorageUpdateOperation operation, string value)
                {
                    var parameters = new Dictionary<string, string>();
                    parameters.Add("key", key);
                    parameters.Add("value", value);
                    parameters.Add("operation", StorageUpdateOperationToString(operation));
                    return new APICall("data-store/update", parameters);
                }

                /// <summary>
                /// Creates an API call that updates a value in the user's data storage.
                /// </summary>
                public static APICall UpdateGlobal(string key, StorageUpdateOperation operation, string value, string username, string token)
                {
                    var parameters = new Dictionary<string, string>();
                    parameters.Add("key", key);
                    parameters.Add("value", value);
                    parameters.Add("operation", StorageUpdateOperationToString(operation));
                    parameters.Add("username", username);
                    parameters.Add("user_token", token);
                    return new APICall("data-store/update", parameters);
                }

                /// <summary>
                /// Creates an API call to set a global data storage entry.
                /// </summary>
                public static APICall SetGlobal(string key, string data)
                {
                    var parameters = new Dictionary<string, string>();
                    parameters.Add("key", key);
                    parameters.Add("data", data);
                    return new APICall("data-store/set", parameters);
                }

                /// <summary>
                /// Creates an API call to set a user's data storage entry.
                /// </summary>
                public static APICall SetUser(string key, string data, string username, string token)
                {
                    var parameters = new Dictionary<string, string>();
                    parameters.Add("key", key);
                    parameters.Add("data", data);
                    parameters.Add("username", username);
                    parameters.Add("user_token", token);
                    return new APICall("data-store/set", parameters);
                }

                /// <summary>
                /// Creates an API call that sets a global data storage item that can only be set by the user that created it.
                /// </summary>
                public static APICall SetRestricted(string key, string data, string username, string token)
                {
                    var parameters = new Dictionary<string, string>();
                    parameters.Add("key", key);
                    parameters.Add("data", data);
                    parameters.Add("restriction_username", username);
                    parameters.Add("restriction_user_token", token);
                    return new APICall("data-store/set", parameters);
                }

                /// <summary>
                /// Creates an API call that removes an entry from the global data storage.
                /// </summary>
                public static APICall RemoveGlobal(string key)
                {
                    var parameters = new Dictionary<string, string>();
                    parameters.Add("key", key);
                    return new APICall("data-store/remove", parameters);
                }

                /// <summary>
                /// Creates an API call that removes an entry from the user's data storage.
                /// </summary>
                public static APICall RemoveGlobal(string key, string username, string token)
                {
                    var parameters = new Dictionary<string, string>();
                    parameters.Add("key", key);
                    parameters.Add("username", username);
                    parameters.Add("user_token", token);
                    return new APICall("data-store/remove", parameters);
                }
            }
        }
    }
}
