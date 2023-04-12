using DataAcess.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataAcess.Repositories
{
    public interface IWarehouseRepository
    {
        public Task AddGoodsToProductionWarehouse(GoodsEntity goods);
        public Task<List<GoodsEntity>> GetAllGoodsInProductionWarehouse();
    }

    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly IMongoDBService _mongoDBService;
        public WarehouseRepository(IMongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task AddGoodsToProductionWarehouse(GoodsEntity goods)
        {
            await _mongoDBService.WarehouseCollection.InsertOneAsync(goods);
        }

        public async Task<List<GoodsEntity>> GetAllGoodsInProductionWarehouse()
        {
            return await _mongoDBService.WarehouseCollection.Find(new BsonDocument()).ToListAsync();
        }
    }
}
