using DataAcess.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataAcess.Repositories
{
    public interface IFactoryModelRepository
    {
        public Task CreateBillOfMaterials(BillOfMaterialsEntity billOfMaterialsEntity);
        public Task CreateFactoryModel(FactoryModelEntity factoryModelEntity);
    }
    public class FactoryModelRepository: IFactoryModelRepository
    {
        private readonly IMongoDBService _mongoDBService;

        public FactoryModelRepository(IMongoDBService mongoDBService) 
        {
            _mongoDBService = mongoDBService;
        }

        public async Task CreateBillOfMaterials(BillOfMaterialsEntity billOfMaterialsEntity)
        {
            await _mongoDBService.BillOfMaterialsCollection.InsertOneAsync(billOfMaterialsEntity);
        }

        public async Task CreateFactoryModel(FactoryModelEntity factoryModelEntity)
        {
            await _mongoDBService.FactoryModelCollection.InsertOneAsync(factoryModelEntity);
        }

        public async Task<List<FactoryModelEntity>> GetFactoryModels()
        {
            return await _mongoDBService.FactoryModelCollection.Find(new BsonDocument()).ToListAsync();
        }
    }
}
