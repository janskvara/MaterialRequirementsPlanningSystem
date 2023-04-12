using DataAcess.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataAcess.Repositories
{
    public interface IComponentInformationRepository
    {
        public Task SaveComponentInformation(ComponentInformationEntity componentInformation);
        public Task SaveProductInformation(ProductInformationEntity productInformation);
        public Task<List<ProductInformationEntity>> GetProductsInformation(List<int> routeSheetIds);
        public Task<ProductInformationEntity> GetProductInformation(string partNumber);
        public Task<ComponentInformationEntity> GetComponentInformation(string partNumber);
    }

    public class ComponentInformationRepository: IComponentInformationRepository
    {
        private readonly IMongoDBService _mongoDBService;

        public ComponentInformationRepository(IMongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task SaveComponentInformation(ComponentInformationEntity componentInformation)
        {
            await _mongoDBService.ComponentInformationCollection.InsertOneAsync(componentInformation);
        }

        public async Task SaveProductInformation(ProductInformationEntity productInformation)
        {
            await _mongoDBService.ProductInformationCollection.InsertOneAsync(productInformation);
        }

        public async Task<List<ProductInformationEntity>> GetProductsInformation(List<int> routeSheetIds)
        {
            var allProducts = await _mongoDBService.ProductInformationCollection.Find(new BsonDocument()).ToListAsync();
            return allProducts.FindAll(x => routeSheetIds.Contains(x.RouteSheetId));
        }

        public async Task<ComponentInformationEntity> GetComponentInformation(string partNumber)
        {
            var allComponents = await _mongoDBService.ComponentInformationCollection.Find(new BsonDocument()).ToListAsync();
            return allComponents.First(x => x.PartNumber == partNumber);
        }

        public async Task<ProductInformationEntity> GetProductInformation(string partNumber)
        {
            var allComponents = await _mongoDBService.ProductInformationCollection.Find(new BsonDocument()).ToListAsync();
            return allComponents.First(x => x.PartNumber == partNumber);
        }
    }
}
