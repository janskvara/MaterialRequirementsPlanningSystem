using DataAcess.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataAcess.Repositories
{
    public interface IMRPRepository
    {
        public Task SaveProductionSheduleReport(ProductionSheduleReportEntity report);
        public Task <Dictionary<string, ProductionSheduleReportEntity>> GetProductionSheduleReports();
        public Task SaveWarehouseOrders(WareHouseOrderEntity wareHouseOrder);
        public Task SaveShiftSummaries(List<ShiftSummaryEntity> shiftSummaries);
        public Task <Dictionary<string, List<ShiftSummaryEntity>>> GetShiftSummaries();
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

        public async Task SaveShiftSummaries(List<ShiftSummaryEntity> shiftSummaries)
        {
            await _mongoDbService.ShiftSummaryCollection.InsertManyAsync(shiftSummaries);
        }

        public async Task<Dictionary<string, List<ShiftSummaryEntity>>> GetShiftSummaries()
        {
            var shifts = await _mongoDbService.ShiftSummaryCollection.Find(new BsonDocument()).ToListAsync();
            return shifts.GroupBy(x => x.ReportId).ToDictionary(group => group.Key, group => group.ToList()); 
        }
    }
}
