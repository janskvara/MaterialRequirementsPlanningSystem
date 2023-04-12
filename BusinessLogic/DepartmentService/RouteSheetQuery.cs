using BusinessLogic.Models;
using DataAcess.Repositories;

namespace BusinessLogic.DepartmentService
{
    public interface IRouteSheetQuery
    {
        public Task<List<RouteSheetModel>> GetRouteSheetsAsync(int departmentId);
        public Task<RouteSheetModel> GetRouteSheetAsync(int routeSheetId);
    }
    public class RouteSheetQuery : IRouteSheetQuery
    {
        private readonly IDepartmentRepository _repository;
        public RouteSheetQuery(IDepartmentRepository capacityPlanningRepository)
        {
            _repository = capacityPlanningRepository;
        }

        public async Task<List<RouteSheetModel>> GetRouteSheetsAsync(int departmentId)
        {
            var routeSheets = await _repository.GetRouteSheetsAsync(departmentId);
            List<RouteSheetModel> routeSheetsModel = new();
            foreach (var routeSheet in routeSheets)
            {
                var routeSheetStations = (await _repository.GetStationsAsync()).FindAll(x => routeSheet.StationList.Contains(x.Id));
                var sequence = 0;
                routeSheetsModel.Add(new RouteSheetModel
                {
                    Id = routeSheet.Id,
                    Name = routeSheet.Name,
                    Department = routeSheet.Department,
                    StationList = routeSheetStations.Select(x => new StationModel()
                    {
                        Sequence = ++sequence,
                        Id = x.Id,
                        Description = x.Description,
                        Instrument = x.Instrument,
                        SetupTimeInSecond = x.SetupTimeInSecond,
                        OperationTimeInSecond = x.OperationTimeInSecond,
                        TransferTimeInSecond = x.TransferTimeInSecond
                    }).ToList()
                });
            }
            return routeSheetsModel;
        }

        public async Task<RouteSheetModel> GetRouteSheetAsync(int routeSheetId)
        {
            var routeSheet = await _repository.GetRouteSheetAsync(routeSheetId);
            var routeSheetStations = (await _repository.GetStationsAsync()).FindAll(x => routeSheet.StationList.Contains(x.Id));
            var sequence = 0;
            return new RouteSheetModel
            {
                Id = routeSheet.Id,
                Name = routeSheet.Name,
                Department = routeSheet.Department,
                StationList = routeSheetStations.Select(x => new StationModel()
                {
                    Sequence = ++sequence,
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
