using MongoDBWebApp.Models;
using MongoDBWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppDatabaseSettings>(
    builder.Configuration.GetSection("AppDatabase"));

builder.Services.AddSingleton<ViewService>();

builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
