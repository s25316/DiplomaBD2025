// Ignore Spelling: Mongo, Enums
using UseCase.MongoDb.UserLogs.Models.ServiceEvents;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserAuthorizationEvents;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.AdminEvents;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.BlockEvents;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RegistrationEvents;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RemoveEvents;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.ResetPasswordEvents;

namespace UseCase.MongoDb.Enums
{
    public static class MongoLogExtensionMethods
    {
        // Properties
        private static Dictionary<MongoLog, Type> _mongoLogsToTypeDictionary = [];

        private static Dictionary<Type, MongoLog> _typeToMongoLogsDictionary = [];


        // Public Methods
        public static Type GetClassType(this MongoLog item)
        {
            if (!_mongoLogsToTypeDictionary.Any())
            {
                Configure();
            }
            return _mongoLogsToTypeDictionary[item];
        }

        /// <summary>
        /// Only for Children BaseLogMongoDb
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static MongoLog GetMongoLog(this Type item)
        {
            if (!_mongoLogsToTypeDictionary.Any())
            {
                Configure();
            }
            return _typeToMongoLogsDictionary[item];
        }

        // Private Methods
        private static void AddToDictionary(Type type, MongoLog mongoLog)
        {
            _mongoLogsToTypeDictionary.Add(mongoLog, type);
            _typeToMongoLogsDictionary.Add(type, mongoLog);
        }

        private static void Configure()
        {
            // ServiceEvents
            AddToDictionary(
                typeof(ApplicationRunMongoDb),
                MongoLog.ApplicationRun);

            // UserAuthorizationEvents
            AddToDictionary(
                typeof(UserAuthorization2StageMongoDb),
                MongoLog.UserAuthorization2Stage);
            AddToDictionary(
                typeof(UserAuthorizationLoginInMongoDb),
                MongoLog.UserAuthorizationLoginIn);
            AddToDictionary(
                typeof(UserAuthorizationLogOutMongoDb),
                MongoLog.UserAuthorizationLogOut);
            AddToDictionary(
                typeof(UserAuthorizationRefreshTokenMongoDb),
                MongoLog.UserAuthorizationRefreshToken);

            // AdminEvents
            AddToDictionary(
                typeof(UserProfileGrantAdminMongoDb),
                MongoLog.UserProfileGrantAdmin);
            AddToDictionary(
                typeof(UserProfileRevokeAdminMongoDb),
                MongoLog.UserProfileRevokeAdmin);

            // BlockEvents
            AddToDictionary(
                typeof(UserProfileBlockedMongoDb),
                MongoLog.UserProfileBlocked);
            AddToDictionary(
                typeof(UserProfileUnBlockedMongoDb),
                MongoLog.UserProfileUnBlocked);

            // RegistrationEvents
            AddToDictionary(
                typeof(UserProfileCreatedMongoDb),
                MongoLog.UserProfileCreated);
            AddToDictionary(
                typeof(UserProfileActivatedMongoDb),
                MongoLog.UserProfileActivated);

            // RemoveEvents
            AddToDictionary(
                typeof(UserProfileRemovedMongoDb),
                MongoLog.UserProfileRemoved);
            AddToDictionary(
                typeof(UserProfileRestoredMongoDb),
                MongoLog.UserProfileRestored);

            // ResetPasswordEvents
            AddToDictionary(
                typeof(UserProfileInitiatedResetPasswordMongoDb),
                MongoLog.UserProfileInitiatedResetPassword);
            AddToDictionary(
                typeof(UserProfileUpdatedPasswordMongoDb),
                MongoLog.UserProfileUpdatedPassword);
        }
    }
}
