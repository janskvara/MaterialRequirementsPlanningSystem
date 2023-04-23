using DataAcess.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DataAcess
{
    public interface IMongoDBService
    {
        public IMongoCollection<StationEntity> StationsCollection { get; }
        public IMongoCollection<RouteSheetEntity> RouteSheetsCollection { get; }
        public IMongoCollection<DepartmentEntity> DeparturesCollection { get; }
        public IMongoCollection<ProductionSheduleReportEntity> ProductionSheduleReportCollection { get; }
        public IMongoCollection<GoodsEntity> WarehouseCollection { get; }
        public IMongoCollection<WareHouseOrderEntity> WarehouseOrderCollection { get; }
        public IMongoCollection<ComponentInformationEntity> ComponentInformationCollection { get; }
        public IMongoCollection<ProductInformationEntity> ProductInformationCollection { get; }
    }

    public class MongoDBService: IMongoDBService
    {
        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            StationsCollection = database.GetCollection<StationEntity>("stations");
            RouteSheetsCollection = database.GetCollection<RouteSheetEntity>("route_sheets");
            DeparturesCollection = database.GetCollection<DepartmentEntity>("department");
            ProductionSheduleReportCollection = database.GetCollection<ProductionSheduleReportEntity>("production-shedule-report");
            WarehouseCollection = database.GetCollection<GoodsEntity>("warehouse");
            WarehouseOrderCollection = database.GetCollection<WareHouseOrderEntity>("warehouse-order");
            ComponentInformationCollection = database.GetCollection<ComponentInformationEntity>("component-information");
            ProductInformationCollection = database.GetCollection<ProductInformationEntity>("product-information");
        }

        public IMongoCollection<StationEntity> StationsCollection { get; }
        public IMongoCollection<RouteSheetEntity> RouteSheetsCollection { get; }
        public IMongoCollection<DepartmentEntity> DeparturesCollection { get; }
        public IMongoCollection<ProductionSheduleReportEntity> ProductionSheduleReportCollection { get; }
        public IMongoCollection<GoodsEntity> WarehouseCollection { get; }
        public IMongoCollection<WareHouseOrderEntity> WarehouseOrderCollection { get; }
        public IMongoCollection<ComponentInformationEntity> ComponentInformationCollection { get; }
        public IMongoCollection<ProductInformationEntity> ProductInformationCollection { get; }
    }
}
