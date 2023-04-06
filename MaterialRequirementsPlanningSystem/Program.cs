using BusinessLogic.DepartmentService;
using BusinessLogic.FactoryModelsService;
using BusinessLogic.MRPService;
using DataAcess;
using DataAcess.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
                //.AddJsonOptions(options =>
                //{
                //    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                //});
builder.Services.AddCors();

//DataAcess
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"))
                .AddSingleton<IMongoDBService, MongoDBService>()
                .AddSingleton<IDepartmentRepository, DepartmentRepository>()
                .AddSingleton<IFactoryModelRepository, FactoryModelRepository>()
                .AddSingleton<IWarehouseRepository, WarehouseRepository>()
                .AddSingleton<IMRPRepository, MRPRepository>();

//BusinessLogic
builder.Services.AddSingleton<ICapacityPlanningQuery, CapacityPlanningQuery>()
                .AddSingleton<IFactoryModelQuery, FactoryModelQuery>()
                .AddSingleton<IProductionShedule, ProductionShedule>()
                .AddSingleton<IDepartmentQuery, DepartmentQuery>();


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
