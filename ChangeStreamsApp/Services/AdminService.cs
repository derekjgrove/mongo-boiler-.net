using ChangeStreamsApp.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ChangeStreamsApp.Services;

public class AdminService
{
    private readonly IMongoDatabase _mongoDatabase;

        public AdminService(IOptions<AppDatabaseSettings> appDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                appDatabaseSettings.Value.ConnectionString);

            _mongoDatabase = mongoClient.GetDatabase(
                appDatabaseSettings.Value.DatabaseName);


            Console.WriteLine("Init AdminService " + appDatabaseSettings.Value.DatabaseName);
        }


    public async Task<BsonDocument> RunAsync(BsonDocument searchIndex)
    {
        // var command = new BsonDocument { { "serverStatus", 1 } };
        Console.WriteLine("searchIndex --> " + searchIndex.ToString());

        try 
        {
            return await _mongoDatabase.RunCommandAsync<BsonDocument>(searchIndex);
        }
        catch (Exception ex)
        {
            Console.WriteLine("error --> " + ex.Message);
        }

        return null;
    }
}