using DataAcess.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataAcess.Repositories
{
    public interface IMRPRepository
    {
        public Task SaveProductionSheduleReport(ProductionSheduleReportEntity report);
        public Task<Dictionary<string, ProductionSheduleReportEntity>> GetProductionSheduleReports();
        public Task SaveWarehouseOrders(WareHouseOrderEntity wareHouseOrder);
    }

    public class MRPRepository: IMRPRepository
    {
        private readonly IMongoDBService _mongoDbService;

        public MRPRepository(IMongoDBService mongoDBService)
        {
            _mongoDbService = mongoDBService;
        }

        public async Task<Dictionary<string, ProductionSheduleReportEntity>> GetProductionSheduleReports()
        {
            
            var reports =  await _mongoDbService.ProductionSheduleReportCollection.Find(new BsonDocument()).ToListAsync();
            return reports.ToDictionary(keySelector: m => m.ReportId, elementSelector: m => m);
        }

        public async Task SaveProductionSheduleReport(ProductionSheduleReportEntity report)
        {
           await _mongoDbService.ProductionSheduleReportCollection.InsertOneAsync(report);
        }

        public async Task SaveWarehouseOrders(WareHouseOrderEntity wareHouseOrder)
        {
            await _mongoDbService.WarehouseOrderCollection.InsertOneAsync(wareHouseOrder);
        }
    }
}
