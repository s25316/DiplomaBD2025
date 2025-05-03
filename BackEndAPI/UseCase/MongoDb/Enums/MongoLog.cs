// Ignore Spelling: enums, Mongo
using System.ComponentModel;

namespace UseCase.MongoDb.Enums
{
    /// <summary>
    /// Kafka Mongo Action
    /// </summary>
    public enum MongoLog
    {
        [Description("Application Run")]
        ApplicationRun = 1,


        // User Profile Events 
        [Description("User Profile: Created")]
        UserProfileCreated = 1001,

        [Description("User Profile: Activated")]
        UserProfileActivated = 1002,

        [Description("User Profile: Removed")]
        UserProfileRemoved = 1003,

        [Description("User Profile: Restored")]
        UserProfileRestored = 1004,

        [Description("User Profile: Blocked")]
        UserProfileBlocked = 1005,

        [Description("User Profile: UnBlocked")]
        UserProfileUnBlocked = 1006,

        [Description("User Profile: Grant Administrator")]
        UserProfileGrantAdmin = 1007,

        [Description("User Profile: Revoke Administrator")]
        UserProfileRevokeAdmin = 1008,

        [Description("User Profile: Updated Password")]
        UserProfileUpdatedPassword = 1011,

        [Description("User Profile: Initiated Reset Password")]
        UserProfileInitiatedResetPassword = 1012,


        // User Authentication Events 
        [Description("User LoginIn")]
        UserAuthorizationLoginIn = 1101,

        [Description("User LogOut")]
        UserAuthorizationLogOut = 1102,

        [Description("User Hand Stage Authorization")]
        UserAuthorization2Stage = 1103,

        [Description("User Refreshed Token")]
        UserAuthorizationRefreshToken = 1104,
    }
}
