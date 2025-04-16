// Ignore spelling: Mongo
using UseCase.MongoDb.Models.UserActions;

namespace UseCase.MongoDb
{
    public interface IMongoDbService
    {
        Task SaveUserAction(UserActionDto action);
    }
}
