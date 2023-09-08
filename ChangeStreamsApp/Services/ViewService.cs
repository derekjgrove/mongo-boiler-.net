using ChangeStreamsApp.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ChangeStreamsApp.Services;

public class ViewService
{
    private readonly IMongoCollection<ViewBO> _viewsCollection;

    // public ViewService(IMongoCollection<BsonDocument> viewsCollection)
    // {
    //     _viewsCollection = viewsCollection.As<ViewBO>();
    // }

        public ViewService(IOptions<AppDatabaseSettings> appDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                appDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                appDatabaseSettings.Value.DatabaseName);

            _viewsCollection = mongoDatabase.GetCollection<ViewBO>(
                appDatabaseSettings.Value.ViewsCollectionName); 

            Console.WriteLine("Init ViewService " + appDatabaseSettings.Value.ConnectionString);
        }

    public async Task<List<ViewBO>> GetAsync()
    {
        return await _viewsCollection.Find(_ => true).ToListAsync();
    }

    public async Task<ViewBO?> GetAsync(string id)
    {
        Console.WriteLine("id --> " + id);
        return await _viewsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(ViewBO newView) =>
        await _viewsCollection.InsertOneAsync(newView);

    public async Task UpdateAsync(string id, ViewBO updatedView) =>
        await _viewsCollection.ReplaceOneAsync(x => x.Id == id, updatedView);

    public async Task RemoveAsync(string id) =>
        await _viewsCollection.DeleteOneAsync(x => x.Id == id);
}