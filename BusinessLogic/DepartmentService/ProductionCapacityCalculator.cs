using BusinessLogic.Models;
using DataAcess.Entities;
using DataAcess.Entities.Enums;

namespace BusinessLogic.DepartmentService
{
    public class ProductionCapacityCalculator
    {
        public static double CalculateProductionCapacityPerWeek(DepartmentEntity department, List<StationModel> stationEntities)
        {
            var productionCapacityPerDay = department.ShiftsPerDay.Sum(x => 
                CalculateProductionCapacityPerShift(x, stationEntities, department.TypeOfProduction));
            return productionCapacityPerDay * department.WorkDaysOfWeek.Count();
        }

        public static int CalculateProductionCapacityPerWorkDay(DepartmentEntity department, List<StationModel> stationEntities)
        {
            return department.ShiftsPerDay.Sum(x => CalculateProductionCapacityPerShift(x, stationEntities, department.TypeOfProduction));
        }

        public static int CalculateProductionCapacityPerShift(Shift shift, List<StationModel> stationEntities, TypeOfProduction productionType)
        {
            return (int)Math.Ceiling(CalculateProductionCapacityPerMinute(productionType, stationEntities) * shift.ShiftDurationInMinutes);
        }

        public static int CalculateProductionCapacityPerShift(ShiftModel shift, List<StationModel> stationEntities, TypeOfProduction productionType)
        {
            return (int)Math.Ceiling(CalculateProductionCapacityPerMinute(productionType, stationEntities) * shift.ShiftDurationInMinutes);
        }

        public static double CalculateProductionCapacityPerMinute(TypeOfProduction productionType, List<StationModel> stationEntities)
        {
            switch (productionType)
            {
                case TypeOfProduction.None:
                case TypeOfProduction.SequentialBatchProduction:
                case TypeOfProduction.SimultaneousBatchProduction:
                case TypeOfProduction.QuantityMassProduction:
                case TypeOfProduction.JobShopWithProductionQuantityOne:
                    return new ProductionRateStrategy().Calculate(stationEntities);
                case TypeOfProduction.FlowLineMassProduction:
                    var productionRatePerSecond = new ProductionRateOfFlowLineMassProductionStrategy().Calculate(stationEntities);
                    return productionRatePerSecond * 60;
            }
            return 0;  
        }

        public static double CalculateMinutesToFinishProduction(TypeOfProduction productionType, List<StationModel> stationEntities, int quantity)
        {
            var productionCapacityPerMinute = CalculateProductionCapacityPerMinute(productionType, stationEntities);
            return Math.Floor(quantity / productionCapacityPerMinute);
        }
    }
}
