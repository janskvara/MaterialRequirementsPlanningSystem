using BusinessLogic.Models;
using DataAcess.Repositories;

namespace BusinessLogic.DepartmentService
{
    public interface ICapacityPlanningQuery
    {
        public Task<RouteSheetResponseModel> GetRouteSheetAsync(int routeSheeId);
    }
    public class CapacityPlanningQuery : ICapacityPlanningQuery
    {
        private readonly IDepartmentRepository _repository;
        public CapacityPlanningQuery(IDepartmentRepository capacityPlanningRepository)
        {
            _repository = capacityPlanningRepository;
        }

        public async Task<RouteSheetResponseModel> GetRouteSheetAsync(int routeSheeId)
        {
            var routeSheet = await _repository.GetRouteSheetAsync(routeSheeId);
            var department = await _repository.GetDepartmentAsync(routeSheet.Departure);
            var routeSheetStations = (await _repository.GetStationsAsync()).FindAll(x => routeSheet.StationList.Contains(x.Id));
           // var productionCapacity = ProductionCapacityCalculator.CalculateProductionCapacityPerWeek(department, routeSheet.StationList);
            var order = 0;
            return new RouteSheetResponseModel
            {
                Id = routeSheet.Id,
                Name = routeSheet.Name,
                Department= department,
               // ProductionCapacityPerWeek = productionCapacity,
                StationList = routeSheetStations.Select(x => new StationModel()
                {
                    Order = ++order,
                    Id = x.Id,
                    Description = x.Description,
                    Instrument = x.Instrument,
                    SetupTimeInSecond = x.SetupTimeInSecond,
                    OperationTimeInSecond = x.OperationTimeInSecond,
                    TransferTimeInSecond = x.TransferTimeInSecond
                }).ToList()
            };
        }
    }
}
