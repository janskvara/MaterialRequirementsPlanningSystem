﻿using DataAcess.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DataAcess
{
    public interface IMongoDBService
    {
        public IMongoCollection<BillOfMaterialsEntity> BillOfMaterialsCollection { get; }
        public IMongoCollection<StationEntity> StationsCollection { get; }
        public IMongoCollection<RouteSheetEntity> RouteSheetsCollection { get; }
        public IMongoCollection<DepartmentEntity> DeparturesCollection { get; }
        public IMongoCollection<FactoryModelEntity> FactoryModelCollection { get; }
        public IMongoCollection<ProductionSheduleReportEntity> ProductionSheduleReportCollection { get; }
        public IMongoCollection<GoodsEntity> WarehouseCollection { get; }
    }

    public class MongoDBService: IMongoDBService
    {

        private readonly IMongoCollection<BillOfMaterialsEntity> _billOfMaterialsCollection;
        private readonly IMongoCollection<StationEntity> _stationCollection;
        private readonly IMongoCollection<RouteSheetEntity> _routeSheetsCollection;
        private readonly IMongoCollection<DepartmentEntity> _departureModelCollection;
        private readonly IMongoCollection<FactoryModelEntity> _factoryModelCollection;
        private readonly IMongoCollection<ProductionSheduleReportEntity> _productionSheduleReportCollection;
        private readonly IMongoCollection<GoodsEntity> _warehouseCollection;

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _billOfMaterialsCollection = database.GetCollection<BillOfMaterialsEntity>("bill_of_materials");
            _stationCollection = database.GetCollection<StationEntity>("stations");
            _routeSheetsCollection = database.GetCollection<RouteSheetEntity>("route_sheets");
            _departureModelCollection = database.GetCollection<DepartmentEntity>("departures");
            _factoryModelCollection = database.GetCollection<FactoryModelEntity>("factory-models");
            _productionSheduleReportCollection = database.GetCollection<ProductionSheduleReportEntity>("production-shedule-report");
            _warehouseCollection = database.GetCollection<GoodsEntity>("warehouse");
        }

        public IMongoCollection<BillOfMaterialsEntity> BillOfMaterialsCollection => _billOfMaterialsCollection;
        public IMongoCollection<StationEntity> StationsCollection => _stationCollection;
        public IMongoCollection<RouteSheetEntity> RouteSheetsCollection => _routeSheetsCollection;
        public IMongoCollection<DepartmentEntity> DeparturesCollection => _departureModelCollection;
        public IMongoCollection<FactoryModelEntity> FactoryModelCollection => _factoryModelCollection;
        public IMongoCollection<ProductionSheduleReportEntity> ProductionSheduleReportCollection => _productionSheduleReportCollection;
        public IMongoCollection<GoodsEntity> WarehouseCollection => _warehouseCollection;
    }
}
