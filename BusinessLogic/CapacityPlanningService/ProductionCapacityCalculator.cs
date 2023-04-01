using DataAcess.Entities;
using DataAcess.Entities.Enums;
using DataAcess.Repositories;

namespace BusinessLogic.CapacityPlanningService
{
    public interface IProductionCapacityCalculator
    {
        public Task<double> CalculateNumberOfShifts(int routeSheeId, int quantity);
    }

    public class ProductionCapacityCalculator: IProductionCapacityCalculator
    {
        private readonly ICapacityPlanningRepository _repository;
        public ProductionCapacityCalculator(ICapacityPlanningRepository repository) 
        {
            _repository= repository;
        }

        internal static double CalculateProductionCapacityPerWeek(DepartmentEntity departure, List<StationEntity> stationEntities)
        {
            return CalculateProductionCapacityPerShift(departure, stationEntities) * departure.WorkDaysPerWeek;
        }

        internal static double CalculateProductionCapacityPerShift(DepartmentEntity departure, List<StationEntity> stationEntities)
        {
            return Math.Floor(CalculateProductionCapacityPerHour(departure, stationEntities) * departure.ShiftDurationInHours);
        }

        internal static double CalculateProductionCapacityPerHour(DepartmentEntity departure, List<StationEntity> stationEntities)
        {
            switch (departure.TypeOfProduction)
            {
                case TypeOfProduction.None:
                case TypeOfProduction.SequentialBatchProduction:
                case TypeOfProduction.SimultaneousBatchProduction:
                case TypeOfProduction.QuantityMassProduction:
                case TypeOfProduction.JobShopWithProductionQuantityOne:
                    return new ProductionRateStrategy().Calculate(stationEntities);
                case TypeOfProduction.FlowLineMassProduction:
                    var productionRatePerSecond = new ProductionRateOfFlowLineMassProductionStrategy().Calculate(stationEntities);
                    return productionRatePerSecond * 60 * 60;
            }
            return 0;  
        }

        public async Task<double> CalculateNumberOfShifts(int routeSheeId, int quantity)
        {
            var routeSheet = await _repository.GetRouteSheetAsync(routeSheeId);
            var department = await _repository.GetDepartmentAsync(routeSheet.Departure);
            var routeSheetStations = (await _repository.GetStationsAsync()).FindAll(x => routeSheet.StationList.Contains(x.Id));
            var productionCapacityPerShift = CalculateProductionCapacityPerShift(department, routeSheetStations);
            return Math.Ceiling(quantity / productionCapacityPerShift);
        }
    }
}
