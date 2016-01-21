using System.Collections.Generic;

namespace Pokemon3D.GameJolt
{
    public partial class API
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
                public static APICall FetchAll(string username, string token)
                {
                    var parameters = new Dictionary<string, string>();
                    parameters.Add("username", username);
                    parameters.Add("user_token", token);
                    return new APICall("trophies", parameters);
                }

                /// <summary>
                /// Creates an API call that returns all achieved trophies for a game.
                /// </summary>
                public static APICall FetchAchieved(string username, string token)
                {
                    var parameters = new Dictionary<string, string>();
                    parameters.Add("achieved", "true");
                    parameters.Add("username", username);
                    parameters.Add("user_token", token);
                    return new APICall("trophies", parameters);
                }

                /// <summary>
                /// Creates an API call that returns a single trophy for a game.
                /// </summary>
                public static APICall Fetch(string trophyId, string username, string token)
                {
                    var parameters = new Dictionary<string, string>();
                    parameters.Add("trophy_id", trophyId);
                    parameters.Add("username", username);
                    parameters.Add("user_token", token);
                    return new APICall("trophies", parameters);
                }

                /// <summary>
                /// Creates an API call that sets the achieved status for a trophy.
                /// </summary>
                public static APICall SetAchieved(bool achieved, string trophyId, string username, string token)
                {
                    var parameters = new Dictionary<string, string>();
                    parameters.Add("trophy_id", trophyId);
                    parameters.Add("username", username);
                    parameters.Add("user_token", token);

                    if (achieved)
                        return new APICall("trophies/add-achieved", parameters);
                    else
                        return new APICall("trophies/remove-achieved", parameters);
                }
            }
        }
    }
}
