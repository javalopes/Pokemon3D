using System.Collections.Generic;

namespace Pokemon3D.GameJolt
{
    // this part of the API class contains methods that create API call instances.

    public partial class Api
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
                public static ApiCall FetchGlobal(string key)
                {
                    var parameters = new Dictionary<string, string> {{"key", key}};
                    return new ApiCall("data-store", parameters);
                }

                /// <summary>
                /// Creates an API call to fetch data from an entry in the user's data storage.
                /// </summary>
                /// <param name="key">The key to return the data from.</param>
                /// <param name="username">The username of the user.</param>
                /// <param name="token">The token of the user.</param>
                public static ApiCall FetchUser(string key, string username, string token)
                {
                    var parameters = new Dictionary<string, string>
                    {
                        {"key", key},
                        {"username", username},
                        {"user_token", token}
                    };
                    return new ApiCall("data-store", parameters);
                }

                /// <summary>
                /// Creates an API call to get all storage keys for this game.
                /// </summary>
                public static ApiCall GetKeysGlobal()
                {
                    return new ApiCall("data-store/get-keys");
                }

                /// <summary>
                /// Creates an API call to get all storage keys for this game that match the given pattern.
                /// </summary>
                public static ApiCall GetKeysGlobal(string pattern)
                {
                    var parameters = new Dictionary<string, string> {{"pattern", pattern}};
                    return new ApiCall("data-store/get-keys", parameters);
                }

                /// <summary>
                /// Creates an API call to get all storage keys for this user.
                /// </summary>
                public static ApiCall GetKeysUser(string username, string token)
                {
                    var parameters = new Dictionary<string, string> {{"username", username}, {"user_token", token}};
                    return new ApiCall("data-store/get-keys", parameters);
                }

                /// <summary>
                /// Creates an API call to get all storage keys for this user that match the given pattern.
                /// </summary>
                public static ApiCall GetKeysUser(string pattern, string username, string token)
                {
                    var parameters = new Dictionary<string, string>
                    {
                        {"pattern", pattern},
                        {"username", username},
                        {"user_token", token}
                    };
                    return new ApiCall("data-store/get-keys", parameters);
                }

                /// <summary>
                /// Creates an API call that updates a value in the global data storage.
                /// </summary>
                public static ApiCall UpdateGlobal(string key, StorageUpdateOperation operation, string value)
                {
                    var parameters = new Dictionary<string, string>
                    {
                        {"key", key},
                        {"value", value},
                        {"operation", StorageUpdateOperationToString(operation)}
                    };
                    return new ApiCall("data-store/update", parameters);
                }

                /// <summary>
                /// Creates an API call that updates a value in the user's data storage.
                /// </summary>
                public static ApiCall UpdateGlobal(string key, StorageUpdateOperation operation, string value, string username, string token)
                {
                    var parameters = new Dictionary<string, string>
                    {
                        {"key", key},
                        {"value", value},
                        {"operation", StorageUpdateOperationToString(operation)},
                        {"username", username},
                        {"user_token", token}
                    };
                    return new ApiCall("data-store/update", parameters);
                }

                /// <summary>
                /// Creates an API call to set a global data storage entry.
                /// </summary>
                public static ApiCall SetGlobal(string key, string data)
                {
                    var parameters = new Dictionary<string, string> {{"key", key}, {"data", data}};
                    return new ApiCall("data-store/set", parameters);
                }

                /// <summary>
                /// Creates an API call to set a user's data storage entry.
                /// </summary>
                public static ApiCall SetUser(string key, string data, string username, string token)
                {
                    var parameters = new Dictionary<string, string>
                    {
                        {"key", key},
                        {"data", data},
                        {"username", username},
                        {"user_token", token}
                    };
                    return new ApiCall("data-store/set", parameters);
                }

                /// <summary>
                /// Creates an API call that sets a global data storage item that can only be set by the user that created it.
                /// </summary>
                public static ApiCall SetRestricted(string key, string data, string username, string token)
                {
                    var parameters = new Dictionary<string, string>
                    {
                        {"key", key},
                        {"data", data},
                        {"restriction_username", username},
                        {"restriction_user_token", token}
                    };
                    return new ApiCall("data-store/set", parameters);
                }

                /// <summary>
                /// Creates an API call that removes an entry from the global data storage.
                /// </summary>
                public static ApiCall RemoveGlobal(string key)
                {
                    var parameters = new Dictionary<string, string> {{"key", key}};
                    return new ApiCall("data-store/remove", parameters);
                }

                /// <summary>
                /// Creates an API call that removes an entry from the user's data storage.
                /// </summary>
                public static ApiCall RemoveGlobal(string key, string username, string token)
                {
                    var parameters = new Dictionary<string, string>
                    {
                        {"key", key},
                        {"username", username},
                        {"user_token", token}
                    };
                    return new ApiCall("data-store/remove", parameters);
                }
            }
        }
    }
}
