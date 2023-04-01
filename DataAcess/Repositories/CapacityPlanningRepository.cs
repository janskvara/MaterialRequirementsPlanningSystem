using DataAcess.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataAcess.Repositories
{
    public interface ICapacityPlanningRepository
    {
        public Task CreateStationAsync(StationEntity newStation);
        public Task<List<StationEntity>> GetStationsAsync();
        public Task CreateRouteSheet(RouteSheetEntity newRouteSheet);
        public Task<RouteSheetEntity> GetRouteSheetAsync(int routeSheetId);
        public Task CreateDepartmentAsync(DepartmentEntity newDeparture);
        public Task<DepartmentEntity> GetDepartmentAsync(int departureId);
    }

    public class CapacityPlanningRepository: ICapacityPlanningRepository
    {
        private readonly IMongoDBService _mongoDBService;
        public CapacityPlanningRepository(IMongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task CreateStationAsync(StationEntity newStation)
        {
            await _mongoDBService.StationsCollection.InsertOneAsync(newStation);
        }

        public async Task<List<StationEntity>> GetStationsAsync()
        {
            return await _mongoDBService.StationsCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task CreateRouteSheet(RouteSheetEntity newRouteSheet)
        {
           await _mongoDBService.RouteSheetsCollection.InsertOneAsync(newRouteSheet);
        }

        public async Task CreateDepartmentAsync(DepartmentEntity newDeparture)
        {
            await _mongoDBService.DeparturesCollection.InsertOneAsync(newDeparture);
        }

        public async Task<RouteSheetEntity> GetRouteSheetAsync(int routeSheetId)
        {
            var routeSheets = await _mongoDBService.RouteSheetsCollection.Find(new BsonDocument()).ToListAsync();
            return routeSheets.First(x => x.Id == routeSheetId);
        }

        public async Task<DepartmentEntity> GetDepartmentAsync(int departmentId)
        {
            var departments = await _mongoDBService.DeparturesCollection.Find(new BsonDocument()).ToListAsync();
            return departments.First(x => x.Id == departmentId);
        }
    }
}
