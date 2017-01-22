using System.Collections.Generic;

namespace Pokemon3D.GameJolt
{
    public partial class Api
    {
        public static partial class Calls
        {
            /// <summary>
            /// Contains static methods to create valid API calls to the Game Jolt API accessing Trophy information.
            /// </summary>
            public static class Trophies
            {
                /// <summary>
                /// Creates an API call that returns all trophies for a game.
                /// </summary>
                public static ApiCall FetchAll(string username, string token)
                {
                    var parameters = new Dictionary<string, string> {{"username", username}, {"user_token", token}};
                    return new ApiCall("trophies", parameters);
                }

                /// <summary>
                /// Creates an API call that returns all achieved trophies for a game.
                /// </summary>
                public static ApiCall FetchAchieved(string username, string token)
                {
                    var parameters = new Dictionary<string, string>
                    {
                        {"achieved", "true"},
                        {"username", username},
                        {"user_token", token}
                    };
                    return new ApiCall("trophies", parameters);
                }

                /// <summary>
                /// Creates an API call that returns a single trophy for a game.
                /// </summary>
                public static ApiCall Fetch(string trophyId, string username, string token)
                {
                    var parameters = new Dictionary<string, string>
                    {
                        {"trophy_id", trophyId},
                        {"username", username},
                        {"user_token", token}
                    };
                    return new ApiCall("trophies", parameters);
                }

                /// <summary>
                /// Creates an API call that sets the achieved status for a trophy.
                /// </summary>
                public static ApiCall SetAchieved(bool achieved, string trophyId, string username, string token)
                {
                    var parameters = new Dictionary<string, string>
                    {
                        {"trophy_id", trophyId},
                        {"username", username},
                        {"user_token", token}
                    };

                    return achieved ? new ApiCall("trophies/add-achieved", parameters) : new ApiCall("trophies/remove-achieved", parameters);
                }
            }
        }
    }
}
