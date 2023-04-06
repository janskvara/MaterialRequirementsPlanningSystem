using DataAcess.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataAcess.Repositories
{
    public interface IDepartmentRepository
    {
        public Task CreateStationAsync(StationEntity newStation);
        public Task<List<StationEntity>> GetStationsAsync();
        public Task CreateRouteSheet(RouteSheetEntity newRouteSheet);
        public Task<RouteSheetEntity> GetRouteSheetAsync(int routeSheetId);
        public Task AddNewDepartmentAsync(DepartmentEntity newDeparture);
        public Task<DepartmentEntity> GetDepartmentAsync(int departureId);
    }

    public class DepartmentRepository: IDepartmentRepository
    {
        private readonly IMongoDBService _mongoDBService;
        public DepartmentRepository(IMongoDBService mongoDBService)
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

        public async Task AddNewDepartmentAsync(DepartmentEntity newDeparture)
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
