using BusinessLogic.ComponentInformationService;
using BusinessLogic.DepartmentService;
using BusinessLogic.MRPService;
using DataAcess;
using DataAcess.Repositories;

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
                .AddSingleton<IWarehouseRepository, WarehouseRepository>()
                .AddSingleton<IMRPRepository, MRPRepository>()
                .AddSingleton<IComponentInformationRepository, ComponentInformationRepository>();

//BusinessLogic
builder.Services.AddSingleton<IRouteSheetQuery, RouteSheetQuery>()
                .AddSingleton<IProductionShedule, ProductionShedule>()
                .AddSingleton<IDepartmentQuery, DepartmentQuery>()
                .AddSingleton<IProductInformationQuery, ProductInformationQuery>();


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
