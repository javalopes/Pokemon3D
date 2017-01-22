using System.Collections.Generic;

namespace Pokemon3D.GameJolt
{
    public partial class Api
    {
        public static partial class Calls
        {
            /// <summary>
            /// Contains static methods to create valid API calls to the Game Jolt API to access score boards.
            /// </summary>
            public static class Scoreboards
            {
                /// <summary>
                /// Creates an API call that returns the information of all score tables for the game.
                /// </summary>
                public static ApiCall FetchAll()
                {
                    return new ApiCall("scores/tables");
                }

                /// <summary>
                /// Creates an API call that returns a single score table.
                /// </summary>
                /// <param name="tableId">The id of the table. If left empty, the primary table gets accessed.</param>
                /// <param name="scoreCount">The amount of scores to return from the table. Maximum is 100.</param>
                public static ApiCall Fetch(string tableId, int scoreCount)
                {
                    // Game Jolt API specification: max amount of scores to be returned is 100.
                    if (scoreCount > 100)
                        scoreCount = 100;

                    var parameters = new Dictionary<string, string>
                    {
                        {"table_id", tableId},
                        {"limit", scoreCount.ToString()}
                    };
                    return new ApiCall("scores", parameters);
                }

                /// <summary>
                /// Creates an API call that returns the rank of a score on a score table.
                /// </summary>
                /// <param name="tableId">The score table to access.</param>
                /// <param name="scoreSortValue">The sort value of the score for which the rank should get returned.</param>
                public static ApiCall FetchScoreRank(string tableId, int scoreSortValue)
                {
                    var parameters = new Dictionary<string, string>
                    {
                        {"table_id", tableId},
                        {"sort", scoreSortValue.ToString()}
                    };
                    return new ApiCall("scores/get-rank", parameters);
                }

                /// <summary>
                /// Creates an API call that registers a score on a score table.
                /// </summary>
                /// <param name="tableId">The id of the score table to access. It will get added to the primary table if left blank.</param>
                /// <param name="score">The score representation (for example "234 jumps")</param>
                /// <param name="scoreSortValue">The sort value of the score (for example "234")</param>
                /// <param name="extraData">Extra data associated with this score. It will not be shown publicly with the score.</param>
                /// <param name="username"></param>
                /// <param name="token"></param>
                public static ApiCall AddScoreUser(string tableId, string score, int scoreSortValue, string extraData, string username, string token)
                {
                    var parameters = new Dictionary<string, string>
                    {
                        {"table_id", tableId},
                        {"score", score},
                        {"sort", scoreSortValue.ToString()},
                        {"extra_data", extraData},
                        {"username", username},
                        {"user_token", token}
                    };
                    return new ApiCall("scores/add", parameters);
                }

                /// <summary>
                /// Creates an API call that registers a guest score on a score table.
                /// </summary>
                /// <param name="tableId">The id of the score table to access. It will get added to the primary table if left blank.</param>
                /// <param name="score">The score representation (for example "234 jumps")</param>
                /// <param name="scoreSortValue">The sort value of the score (for example "234")</param>
                /// <param name="guestName">The name of the guest.</param>
                /// <param name="extraData">Extra data associated with this score. It will not be shown publicly with the score.</param>
                public static ApiCall AddScoreGuest(string tableId, string score, int scoreSortValue, string guestName, string extraData)
                {
                    var parameters = new Dictionary<string, string>
                    {
                        {"table_id", tableId},
                        {"score", score},
                        {"sort", scoreSortValue.ToString()},
                        {"extra_data", extraData},
                        {"guest", guestName}
                    };
                    return new ApiCall("scores/add", parameters);
                }
            }
        }
    }
}
