using MongoDB.Driver;

namespace BeireMKit.Data.Interfaces.MongoDB
{
    public interface IBaseMongoDbContext
    {
        IMongoDatabase Database { get; }
    }
}
