using BusinessLogic.Models;
using DataAcess.Repositories;

namespace BusinessLogic.CapacityPlanningService
{
    public interface ICapacityPlanningQuery
    {
        public Task<RouteSheetResponseModel> GetRouteSheetAsync(int routeSheeId);
    }
    public class CapacityPlanningQuery : ICapacityPlanningQuery
    {
        private readonly ICapacityPlanningRepository _repository;
        public CapacityPlanningQuery(ICapacityPlanningRepository capacityPlanningRepository)
        {
            _repository = capacityPlanningRepository;
        }

        public async Task<RouteSheetResponseModel> GetRouteSheetAsync(int routeSheeId)
        {
            var routeSheet = await _repository.GetRouteSheetAsync(routeSheeId);
            var department = await _repository.GetDepartmentAsync(routeSheet.Departure);
            var routeSheetStations = (await _repository.GetStationsAsync()).FindAll(x => routeSheet.StationList.Contains(x.Id));
            var productionCapacity = ProductionCapacityCalculator.CalculateProductionCapacityPerWeek(department, routeSheetStations);
            var order = 0;
            return new RouteSheetResponseModel
            {
                Id = routeSheet.Id,
                Name = routeSheet.Name,
                ProductionCapacityPerWeek = productionCapacity,
                Department = new DepartmentModel
                {
                    Id = department.Id,
                    Name = department.Name,
                    ShiftDurationInHours = department.ShiftDurationInHours,
                    WorkDaysPerWeek = department.WorkDaysPerWeek,
                    TypeOfProduction = department.TypeOfProduction.ToString(),
                },
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
