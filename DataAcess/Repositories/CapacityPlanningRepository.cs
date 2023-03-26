using DataAcess.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataAcess.Repositories
{
    public interface ICapacityPlanningRepository
    {
        public Task CreateStation(StationEntity newStation);
        public Task<List<StationEntity>> GetStations();
        public Task CreateRouteSheet(RouteSheetEntity newRouteSheet);
        public Task<RouteSheetEntity> GetRouteSheet(int routeSheetId);
        public Task CreateDepartment(DepartmentEntity newDeparture);
        public Task<DepartmentEntity> GetDepartment(int departureId);
    }

    public class CapacityPlanningRepository: ICapacityPlanningRepository
    {
        private readonly IMongoDBService _mongoDBService;
        public CapacityPlanningRepository(IMongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task CreateStation(StationEntity newStation)
        {
            await _mongoDBService.StationsCollection.InsertOneAsync(newStation);
        }

        public async Task<List<StationEntity>> GetStations()
        {
            return await _mongoDBService.StationsCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task CreateRouteSheet(RouteSheetEntity newRouteSheet)
        {
           await _mongoDBService.RouteSheetsCollection.InsertOneAsync(newRouteSheet);
        }

        public async Task CreateDepartment(DepartmentEntity newDeparture)
        {
            await _mongoDBService.DeparturesCollection.InsertOneAsync(newDeparture);
        }

        public async Task<RouteSheetEntity> GetRouteSheet(int routeSheetId)
        {
            var routeSheets = await _mongoDBService.RouteSheetsCollection.Find(new BsonDocument()).ToListAsync();
            return routeSheets.First(x => x.Id == routeSheetId);
        }

        public async Task<DepartmentEntity> GetDepartment(int departmentId)
        {
            var departments = await _mongoDBService.DeparturesCollection.Find(new BsonDocument()).ToListAsync();
            return departments.First(x => x.Id == departmentId);
        }
    }
}
