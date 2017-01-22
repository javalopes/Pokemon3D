using System.Collections.Generic;

namespace Pokemon3D.GameJolt
{
    public partial class Api
    {
        public static partial class Calls
        {
            /// <summary>
            /// Contains static methods to create valid API calls to the Game Jolt API to work with friendlists.
            /// </summary>
            public static class Friends
            {
                /// <summary>
                /// Creates an API call that returns all sent friend requests for a user.
                /// </summary>
                public static ApiCall FetchSentFriendRequests(string username, string token)
                {
                    var parameters = new Dictionary<string, string> {{"username", username}, {"user_token", token}};
                    return new ApiCall("friends/sent-requests", parameters);
                }

                /// <summary>
                /// Creates an API call that returns all received friend requests for a user.
                /// </summary>
                public static ApiCall FetchReceivedFriendRequests(string username, string token)
                {
                    var parameters = new Dictionary<string, string> {{"username", username}, {"user_token", token}};
                    return new ApiCall("friends/received-requests", parameters);
                }

                /// <summary>
                /// Creates an API call that returns the friend list for a user.
                /// </summary>
                /// <param name="userId">The user id of the owner of the friend list.</param>
                public static ApiCall FetchFriendList(string userId)
                {
                    var parameters = new Dictionary<string, string> {{"user_id", userId}};
                    return new ApiCall("friends", parameters);
                }

                /// <summary>
                /// Creates an API call that sends a friend request to another user.
                /// </summary>
                /// <param name="targetUserId">The target user id of the friend request.</param>
                /// <param name="username"></param>
                /// <param name="token"></param>
                public static ApiCall SendFriendRequest(string targetUserId, string username, string token)
                {
                    var parameters = new Dictionary<string, string>
                    {
                        {"target_user_id", targetUserId},
                        {"username", username},
                        {"user_token", token}
                    };
                    return new ApiCall("friends/send-request", parameters);
                }

                /// <summary>
                /// Creates an API call that cancels a friend request to another user.
                /// </summary>
                /// <param name="targetUserId">The target user id of the friend request.</param>
                /// <param name="username"></param>
                /// <param name="token"></param>
                public static ApiCall CancelFriendRequest(string targetUserId, string username, string token)
                {
                    var parameters = new Dictionary<string, string>
                    {
                        {"target_user_id", targetUserId},
                        {"username", username},
                        {"user_token", token}
                    };
                    return new ApiCall("friends/cancel-request", parameters);
                }

                /// <summary>
                /// Creates an API call that accepts a friend request from another user.
                /// </summary>
                /// <param name="targetUserId">The sender user id of the friend request.</param>
                /// <param name="username"></param>
                /// <param name="token"></param>
                public static ApiCall AcceptFriendRequest(string targetUserId, string username, string token)
                {
                    var parameters = new Dictionary<string, string>
                    {
                        {"target_user_id", targetUserId},
                        {"username", username},
                        {"user_token", token}
                    };
                    return new ApiCall("friends/accept-request", parameters);
                }

                /// <summary>
                /// Creates an API call that declines a friend request from another user.
                /// </summary>
                /// <param name="targetUserId">The sender user id of the friend request.</param>
                /// <param name="username"></param>
                /// <param name="token"></param>
                public static ApiCall DeclineFriendRequest(string targetUserId, string username, string token)
                {
                    var parameters = new Dictionary<string, string>
                    {
                        {"target_user_id", targetUserId},
                        {"username", username},
                        {"user_token", token}
                    };
                    return new ApiCall("friends/decline-request", parameters);
                }
            }
        }
    }
}
