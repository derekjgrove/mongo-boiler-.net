namespace MongoDBWebApp.Models;

public class AppDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string ViewsCollectionName { get; set; } = null!;
}