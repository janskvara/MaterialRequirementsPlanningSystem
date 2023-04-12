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

        private readonly IMongoCollection<StationEntity> _stationCollection;
        private readonly IMongoCollection<RouteSheetEntity> _routeSheetsCollection;
        private readonly IMongoCollection<DepartmentEntity> _departureModelCollection;
        private readonly IMongoCollection<ProductionSheduleReportEntity> _productionSheduleReportCollection;
        private readonly IMongoCollection<GoodsEntity> _warehouseCollection;
        private readonly IMongoCollection<WareHouseOrderEntity> _warehouseOrderCollection;
        private readonly IMongoCollection<ComponentInformationEntity> _componentInformationCollection;
        private readonly IMongoCollection<ProductInformationEntity> _productInformationCollection;

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _stationCollection = database.GetCollection<StationEntity>("stations");
            _routeSheetsCollection = database.GetCollection<RouteSheetEntity>("route_sheets");
            _departureModelCollection = database.GetCollection<DepartmentEntity>("department");
            _productionSheduleReportCollection = database.GetCollection<ProductionSheduleReportEntity>("production-shedule-report");
            _warehouseCollection = database.GetCollection<GoodsEntity>("warehouse");
            _warehouseOrderCollection = database.GetCollection<WareHouseOrderEntity>("warehouse-order");
            _componentInformationCollection = database.GetCollection<ComponentInformationEntity>("component-information");
            _productInformationCollection = database.GetCollection<ProductInformationEntity>("product-information");
        }

        public IMongoCollection<StationEntity> StationsCollection => _stationCollection;
        public IMongoCollection<RouteSheetEntity> RouteSheetsCollection => _routeSheetsCollection;
        public IMongoCollection<DepartmentEntity> DeparturesCollection => _departureModelCollection;
        public IMongoCollection<ProductionSheduleReportEntity> ProductionSheduleReportCollection => _productionSheduleReportCollection;
        public IMongoCollection<GoodsEntity> WarehouseCollection => _warehouseCollection;
        public IMongoCollection<WareHouseOrderEntity> WarehouseOrderCollection => _warehouseOrderCollection;
        public IMongoCollection<ComponentInformationEntity> ComponentInformationCollection => _componentInformationCollection;
        public IMongoCollection<ProductInformationEntity> ProductInformationCollection => _productInformationCollection;
    }
}
