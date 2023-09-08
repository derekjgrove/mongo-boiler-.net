using ChangeStreamsApp;
using ChangeStreamsApp.Models;
using ChangeStreamsApp.Services;
using MongoDB.Driver;


IHost host = (IHost)Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.SetBasePath(AppContext.BaseDirectory);
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.Configure<AppDatabaseSettings>(hostContext.Configuration.GetSection("AppDatabase"));
        services.AddSingleton<IMongoClient, MongoClient>();
        // services.AddHostedService<Worker>();
        services.AddHostedService<WorkerService>();
        services.AddSingleton<ViewService>();
        services.AddSingleton<AdminService>();
    })
    .ConfigureLogging((hostContext, configLogging) =>
    {
        configLogging.AddConsole(); // Add console logger
    })
    .Build();

host.Run();
