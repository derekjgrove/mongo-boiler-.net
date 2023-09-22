using MongoDBWebApp.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MongoDBWebApp.Services;

public class ViewService
{
    private readonly IMongoCollection<ViewBO> _viewsCollection;

    public ViewService(
        IOptions<AppDatabaseSettings> appDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            appDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            appDatabaseSettings.Value.DatabaseName);

        _viewsCollection = mongoDatabase.GetCollection<ViewBO>(
            appDatabaseSettings.Value.ViewsCollectionName);
    }

    public async Task<List<ViewBO>> GetAsync() =>
        await _viewsCollection.Find(_ => true).ToListAsync();

    public async Task<ViewBO?> GetAsync(string id) =>
        await _viewsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(ViewBO newView) =>
        await _viewsCollection.InsertOneAsync(newView);

    public async Task UpdateAsync(string id, string type, FieldBO updatedView)
    {
        var filter = Builders<ViewBO>.Filter.Eq("_id", ObjectId.Parse(id));
        var update = string.Equals(type, "push", StringComparison.OrdinalIgnoreCase) ?
            Builders<ViewBO>.Update.Push("fields", updatedView)
        :
            Builders<ViewBO>.Update.PullFilter("fields", Builders<BsonDocument>.Filter.Eq("attrName", updatedView.AttrName));

        await _viewsCollection.UpdateOneAsync(filter, update);
    }

    public async Task RemoveAsync(string id) =>
        await _viewsCollection.DeleteOneAsync(x => x.Id == id);
}