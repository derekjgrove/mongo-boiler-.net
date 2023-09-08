using ChangeStreamsApp.Models;
using ChangeStreamsApp.Services;
using MongoDB.Bson;
using MongoDB.Driver;
namespace ChangeStreamsApp;




public class Worker
{
    private readonly IMongoCollection<BsonDocument> _collection;
    private readonly CancellationToken _cancellationToken;
    private Task _workerTask;
    private readonly ILogger<Worker> _logger;

    private readonly ViewService _viewService;
    private readonly AdminService _adminService;


    public Worker(ViewService viewService, AdminService adminService, IMongoCollection<BsonDocument> collection, CancellationToken cancellationToken)
    {
        _collection = collection;
        _cancellationToken = cancellationToken;
        _viewService = viewService;
        _adminService = adminService;
    }

    private async Task insertHelperAsync(string id) {
        var view = await _viewService.GetAsync(id);

        Console.WriteLine("view result " + view);
        if (view is null)
        {
            // return NotFound();
            Console.WriteLine("Not Found");
        } else
        {
            SearchIndexBO searchIndexBO = new SearchIndexBO(_collection.CollectionNamespace.ToString(), view.Fields);
            // Console.WriteLine("searchdefinition " + searchIndexBO.getSearchIndex());
            var res = await _adminService.RunAsync(searchIndexBO.getSearchIndex());
            Console.WriteLine("result --> " + res.ToString());
        }

        
    }
    public void Start()
    {
        Console.WriteLine($"Worker Start Init at: {DateTimeOffset.Now}");
        _workerTask = Task.Run(async () =>
        {
            Console.WriteLine($"Worker running at: {DateTimeOffset.Now}");
            var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<BsonDocument>>();


            var options = new ChangeStreamOptions
            {
                FullDocument = ChangeStreamFullDocumentOption.UpdateLookup,
                ShowExpandedEvents = true
            };

            using (var cursor = await _collection.WatchAsync(pipeline, options, _cancellationToken))
            {
                Console.WriteLine($"Watching Collection({_collection.CollectionNamespace}) at: "+ $"{DateTimeOffset.Now}");

                await cursor.ForEachAsync(async change =>
                {

                    Console.WriteLine($"Change triggered ({change.OperationType}) at: " + $"{DateTimeOffset.Now}");                    
                    
                    switch (change.OperationType)
                    {
                        case ChangeStreamOperationType.Update:
                            //Update changes
                            break;
                        case ChangeStreamOperationType.Insert:
                            Console.WriteLine($"Inserted DocumentKey - {change.DocumentKey}");
                            Console.WriteLine($"Inserted Document - {change.FullDocument.ToJson()}");

                            await insertHelperAsync(change.DocumentKey.GetValue("_id").ToString());
                            //Insert changes
                            break;
                        case ChangeStreamOperationType.Delete:
                            //Delete changes
                            break;
                        default:
                            // nothing
                            break;
                    }
                   

                });
            }
        });


    }

    public void Stop()
    {
        if (_workerTask != null && !_workerTask.IsCompleted)
        {
            _workerTask.Wait();
        }
    }
}