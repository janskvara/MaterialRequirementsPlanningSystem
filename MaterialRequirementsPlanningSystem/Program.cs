using BusinessLogic.CapacityPlanningService;
using DataAcess;
using DataAcess.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors();

//DataAcess
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"))
                .AddSingleton<IMongoDBService, MongoDBService>()
                .AddSingleton<ICapacityPlanningRepository, CapacityPlanningRepository>()
                .AddSingleton<IFactoryModelRepository, FactoryModelRepository>();

//BusinessLogic
builder.Services.AddSingleton<ICapacityPlanningQuery, CapacityPlanningQuery>();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
