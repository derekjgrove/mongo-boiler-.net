namespace ChangeStreamsApp.Models;

public class AppDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string AdminDatabase { get; set; } = null!;

    public string ViewsCollectionName { get; set; } = null!;

    public List<CollectionConfig> Collections { get; set; }
}

    public class CollectionConfig
    {
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }