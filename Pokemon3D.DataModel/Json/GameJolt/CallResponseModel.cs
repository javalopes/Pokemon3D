using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameJolt
{
    [DataContract]
    public abstract class CallResponseModel : DataModel<CallResponseModel>
    {
        /// <summary>
        /// Used by all API endpoints.
        /// Indicates wether the call was successful or not.
        /// </summary>
        [DataMember(Name = "success")]
        public bool Success;

        /// <summary>
        /// Only filled if the call returned unsuccessfully. This contains the error message.
        /// </summary>
        [DataMember(Name = "message")]
        public string Message;

        /// <summary>
        /// Contains data returned from data storage operations (get and update).
        /// </summary>
        [DataMember(Name = "data")]
        public string Data;

        /// <summary>
        /// The list of keys returned from data storage key get operations.
        /// </summary>
        [DataMember(Name = "keys")]
        public KeyModel[] Keys;

        #region GetTime

        /// <summary>
        /// Returned from get-time. The Game Jolt server's UNIX time stamp.
        /// </summary>
        [DataMember(Name = "timestamp")]
        public int Timestamp;

        /// <summary>
        /// Returned from get-time. The Game Jolt server's timezone.
        /// </summary>
        [DataMember(Name = "timezone")]
        public string Timezone;

        /// <summary>
        /// Returned from get-time. The Game Jolt server's current year.
        /// </summary>
        [DataMember(Name = "year")]
        public int Year;

        /// <summary>
        /// Returned from get-time. The Game Jolt server's current month.
        /// </summary>
        [DataMember(Name = "month")]
        public int Month;

        /// <summary>
        /// Returned from get-time. The Game Jolt server's current day.
        /// </summary>
        [DataMember(Name = "day")]
        public int Day;

        /// <summary>
        /// Returned from get-time. The Game Jolt server's current hour.
        /// </summary>
        [DataMember(Name = "hour")]
        public int Hour;

        /// <summary>
        /// Returned from get-time. The Game Jolt server's current minute.
        /// </summary>
        [DataMember(Name = "minute")]
        public int Minute;

        /// <summary>
        /// Returned from get-time. The Game Jolt server's current second.
        /// </summary>
        [DataMember(Name = "second")]
        public int Second;

        #endregion

        #region Scoreboards

        /// <summary>
        /// The rank of a score on a scoreboard returned by get-rank.
        /// </summary>
        [DataMember(Name = "rank")]
        public int Rank;

        /// <summary>
        /// The score string returned by a scoreboard fetch operation.
        /// </summary>
        [DataMember(Name = "score")]
        public string Score;

        /// <summary>
        /// The score's sort value returned by a scoreboard fetch operation.
        /// </summary>
        [DataMember(Name = "sort")]
        public int Sort;

        /// <summary>
        /// Any extra data associated with the score returned by a scoreboard fetch operation.
        /// </summary>
        [DataMember(Name = "extra_data")]
        public string ExtraData;

        /// <summary>
        /// Contains the user name.
        /// Used by the scoreboard fetch operation, where it contains the user name of a score (if it is a user score).
        /// </summary>
        [DataMember(Name = "user")]
        public string User;

        /// <summary>
        /// Contains the user id.
        /// Used by the scoreboard fetch operation, where it contains the user id of a score (if it is a user score).
        /// </summary>
        [DataMember(Name = "user_id")]
        public int UserId;

        /// <summary>
        /// Contains a guest's name.
        /// Used by the scoreboard fetch operation, where it contains the guest's name, if the score is not a user score.
        /// </summary>
        [DataMember(Name = "guest")]
        public string Guest;

        /// <summary>
        /// The data a score was stored on the scoreboard.
        /// </summary>
        [DataMember(Name = "stored")]
        public string Stored;

        /// <summary>
        /// The score tables for a game returned by a scoreboard tables operation.
        /// </summary>
        [DataMember(Name = "tables")]
        public ScoreTableModel[] Tables;

        #endregion

        /// <summary>
        /// The list of trophies returned from a trophies fetch operation.
        /// </summary>
        [DataMember(Name = "trophies")]
        public TrophyModel[] Trophies;

        /// <summary>
        /// The list of users returned by a user fetch call.
        /// </summary>
        [DataMember(Name = "users")]
        public UserModel[] Users;
    }
}
