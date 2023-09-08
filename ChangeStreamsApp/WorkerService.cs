using ChangeStreamsApp.Models;
using ChangeStreamsApp.Services;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ChangeStreamsApp;

    public class WorkerService : BackgroundService
    {
        private readonly AppDatabaseSettings _mongoDBSettings;
        private readonly IMongoClient _mongoClient;
        private readonly List<Worker> _changeStreamWorkers;
        private readonly ViewService _viewService;
        private readonly AdminService _adminService;

        public WorkerService(IOptions<AppDatabaseSettings> mongoDBSettings, IMongoClient mongoClient, ViewService viewService, AdminService adminService )
        {
            _mongoDBSettings = mongoDBSettings.Value;
            // _mongoClient = mongoClient;
            _mongoClient = new MongoClient(_mongoDBSettings.ConnectionString);
            _changeStreamWorkers = new List<Worker>();
            _viewService = viewService;
            _adminService = adminService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            foreach (var collectionConfig in _mongoDBSettings.Collections)
            {
                var database = _mongoClient.GetDatabase(collectionConfig.DatabaseName);
                var collection = database.GetCollection<BsonDocument>(collectionConfig.CollectionName);
                var changeStreamWorker = new Worker(_viewService, _adminService, collection, stoppingToken);
                _changeStreamWorkers.Add(changeStreamWorker);
                changeStreamWorker.Start();
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken); // Delay for 30 seconds before checking again
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var changeStreamWorker in _changeStreamWorkers)
            {
                changeStreamWorker.Stop();
            }

            await base.StopAsync(cancellationToken);
        }
    }

