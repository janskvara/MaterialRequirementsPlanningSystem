using BusinessLogic.DepartmentService;
using BusinessLogic.FactoryModelsService;
using BusinessLogic.Models;
using DataAcess.Entities;
using DataAcess.Repositories;
using MongoDB.Driver;

namespace BusinessLogic.MRPService
{
    public interface IProductionShedule
    {
        public Task Plan(OrderPlanRequest productionPlan);
        public Task<ProductionSheduleReportsModel> GetReports();
    }

    public class ProductionShedule: IProductionShedule
    {
        private readonly IFactoryModelQuery _factoryModelQuery;
        private readonly IMRPRepository _mrpRepository;
        private readonly IWarehouseRepository _warehouseRepository;

        public ProductionShedule(IFactoryModelQuery factoryModelQuery, IMRPRepository mrpRepository, IWarehouseRepository warehouseRepository)
        {
            _factoryModelQuery= factoryModelQuery;
            _mrpRepository= mrpRepository;
            _warehouseRepository= warehouseRepository;
        }

        public async Task Plan(OrderPlanRequest productionPlan)
        {
            var logs = new List<string>();
            var events = new List<EventEntity>();
            var productionEvents = new List<ManufacturesEventModel>();

            var shiftSummaryManager = new ShiftSummaryManager(productionPlan.ReportId);
            var orderedOrders = productionPlan.Orders.OrderBy(x => x.ExportDate).Reverse().ToList();
            foreach (var order in orderedOrders)
            {
                var model = await _factoryModelQuery.GetFactoryModelAsync(order.ModelId);
                var department = model.RouteSheet.Department;
                var shiftsFromLatest = department.ShiftsPerDay.OrderBy(x => x.EndTime).Reverse().ToList();

                DateTime orderFinishDate = GetAviableDate(department.WorkDaysOfWeek, order.ExportDate.AddDays(-1));
                DateTime orderFinishTime = CombineDateTimeWithDateWithTimeSpan(orderFinishDate, shiftsFromLatest.First().EndTime);

                var actualQuantity = 0;
                ShiftModel? shift = null;
                if(shiftSummaryManager.ShiftSummaries.Count == 0 || shiftSummaryManager.GetLastShift().ShiftStart.Date > orderFinishTime.Date)
                {
                    var shiftDate = GetAviableDate(department.WorkDaysOfWeek, order.ExportDate.AddDays(-1));
                    var startTime = CombineDateTimeWithDateWithTimeSpan(shiftDate, shiftsFromLatest.First().StartTime);
                    var endTime = CombineDateTimeWithDateWithTimeSpan(shiftDate, shiftsFromLatest.First().EndTime);
                    shift = new ShiftModel(startTime, endTime);
                }


                var orderIsFinish = false;
                while (!orderIsFinish)
                {
                    shift = shift ?? GetAviableShift(shiftsFromLatest, shiftSummaryManager, department.WorkDaysOfWeek);
                    
                    if (shift.StartTime.Date < productionPlan.FromDate.Date)
                            break;
                   
                    var quantityPerShift = ProductionCapacityCalculator.CalculateProductionCapacityPerShift(shift, model.RouteSheet.StationList, department.TypeOfProduction);
                    actualQuantity += quantityPerShift;

                    if (actualQuantity < order.Quantity)
                    {
                        shiftSummaryManager.AddShift(model, order, quantityPerShift, shift.EndTime.AddMinutes(-shift.ShiftDurationInMinutes), shift.EndTime);
                        shift = null;
                        continue;
                    }

                    if (actualQuantity == order.Quantity)
                    {
                        var startDate = shift.EndTime.AddMinutes(-shift.ShiftDurationInMinutes);
                        shiftSummaryManager.AddShift(model, order, quantityPerShift, startDate, shift.EndTime);
                    }
                    else
                    {
                        var quantityInUnfinishedShift = quantityPerShift - (actualQuantity - order.Quantity);
                        var minutesToFinishOrder = ProductionCapacityCalculator.CalculateMinutesToFinishProduction(department.TypeOfProduction,
                                model.RouteSheet.StationList, quantityInUnfinishedShift);
                        var startDate = shift.EndTime.AddMinutes(-minutesToFinishOrder);
                        shiftSummaryManager.AddShift(model, order, quantityInUnfinishedShift, startDate, shift.EndTime);
                    }
                    orderIsFinish = true;
                    shift = null;
                }

                if (orderIsFinish)
                {
                    var shifts = shiftSummaryManager.GetShifts(order.OrderId).OrderBy(x => x.ShiftStart);
                    var totalQuantity = shifts.Sum(x => x.Quantity);
                    var productionEvent = new ManufacturesEventModel(order.OrderId, model.Name, totalQuantity, shifts.First().ShiftStart, shifts.Last().ShiftEnd);
                    productionEvents.Add(productionEvent);
                    events.Add(new ExportEventModel(order.OrderId, model.Name, order.ExportDate).Event);
                    events.Add(productionEvent.Event);
                }
                else
                {
                    logs.Add($"Kapacitne planovanie obmedzuje vykonat objednavku:{order.OrderId}. Zaciatok vyroby" +
                            $"by bol pred datumom zacatia planu: {productionPlan.FromDate}.");
                    shiftSummaryManager.RemoveShifts(order.OrderId);
                }
            }


            var orderedProductionEvents = productionEvents.OrderBy(x => x.Event.StartTime).ToList();
            var changesInWareHouse = new ChangesInWarehouseModel();
            var orders = new List<OrderGoodsEntities>();
            foreach (var productionEvent in orderedProductionEvents)
            {
                var order = productionPlan.Orders.First(x => x.OrderId == productionEvent.OrderId);
                var model = await _factoryModelQuery.GetFactoryModelAsync(order.ModelId);

                var wholeBill = model.SummarryMaterialList.Select(s => new ProductRequirementModel
                {
                    PartNumber = s.PartNumber,
                    Quantity = s.Quantity * order.Quantity,
                    ModelType = s.ModelType,
                    Description = s.Description,
                    Unit = s.Unit
                }).ToList();
                var goodsList = (await _warehouseRepository.GetAllGoodsInProductionWarehouse())
                    .FindAll(s => wholeBill.Any(x => x.PartNumber.Equals(s.PartNumber)));

                //inicializacia zmien vo warehouse -> zaciname aktualnym stavov na sklade
                foreach (var goods in goodsList)
                {
                    changesInWareHouse.AddGoodsChange(goods.PartNumber, productionPlan.FromDate, 0, goods.ActualCapacity);
                }

                var orderGoodsEntity = new OrderGoodsEntities(productionEvent.Event.StartTime.AddDays(-1));
                foreach (var component in wholeBill)
                {
                    var componentInWarehouse = goodsList.First(x => x.PartNumber.Equals(component.PartNumber));
                    var lastChangeOfTheComponent = changesInWareHouse.ChangesOfProducts[component.PartNumber].Last();
                    var newCapacity = lastChangeOfTheComponent.NewCapacity - component.Quantity;

                    //ak by nebolo dostatok vyrobkov po vyrobe -> znamena ze este pred vyrobov potrebujeme vytovrit objednavku
                    //treba porozmyslat co s maxCapacity ak sa prekroci pre danu objednavku mu sa objednavka rozdelit na viac objednavok
                    if (newCapacity < componentInWarehouse.MinimumThresholdCapacity)
                    {
                        var count = componentInWarehouse.MinimumThresholdCapacity - newCapacity;
                        orderGoodsEntity.Orders.Add(new Order(component.PartNumber, count));
                        var updatedCapacity = componentInWarehouse.ActualCapacity + count;
                        changesInWareHouse.AddGoodsChange(component.PartNumber, orderGoodsEntity.ImportDate,
                            count, updatedCapacity);
                    }

                    lastChangeOfTheComponent = changesInWareHouse.ChangesOfProducts[component.PartNumber].Last();
                    var capacityAfterProductionOrder = lastChangeOfTheComponent.NewCapacity - component.Quantity;
                    changesInWareHouse.AddGoodsChange(component.PartNumber, productionEvent.Event.EndTime,
                        component.Quantity, capacityAfterProductionOrder);
                }

                if (orderGoodsEntity.Orders.Any())
                {
                    await _mrpRepository.SaveWarehouseOrders(new WareHouseOrderEntity(productionPlan.ReportId, productionEvent.OrderId, orderGoodsEntity));
                    var orderEvent = new ImportGoodsEventModel(order.OrderId, model.Id, orderGoodsEntity.ImportDate, orderGoodsEntity.Orders);
                    events.Add(orderEvent.Event);
                    logs.Add($"Bude nutne doobjednat tovar pre:{order.OrderId}. Deadline: {orderGoodsEntity.ImportDate}. Podrobnosti v danom evente.");
                }
                else
                {
                    logs.Add($"Nie je potrebne doobjednavat ziaden tovar pre objednavku:{order.OrderId}.");
                }
            }

            var report = new ProductionSheduleReportEntity
            {
                ReportId = productionPlan.ReportId,
                FromDate = productionPlan.FromDate,
                Orders = productionPlan.Orders,
                Events = events,
                ShiftSummaries= shiftSummaryManager.ShiftSummaries,
                Description = logs
            };
            await _mrpRepository.SaveProductionSheduleReport(report);
        }

        private DateTime CombineDateTimeWithDateWithTimeSpan(DateTime date, TimeSpan timeOfDay)
        {
            return new DateTime(date.Year, date.Month, date.Day, 
                timeOfDay.Hours, timeOfDay.Minutes, timeOfDay.Seconds, DateTimeKind.Utc);
        }

        private ShiftModel GetAviableShift(List<Shift> shiftsFromLatest, ShiftSummaryManager shiftSummaryManager, List<DayOfWeek> workWeekDays)
        {
            var lastOccupiedShift = shiftSummaryManager.GetLastShift();
            //new day - return last shift from new day
            if (shiftsFromLatest.Last().StartTime == lastOccupiedShift.ShiftStart.TimeOfDay)
            {
                var shiftDate = GetAviableDate(workWeekDays, shiftSummaryManager.GetLastShift().ShiftStart.AddDays(-1));
                var startTime = CombineDateTimeWithDateWithTimeSpan(shiftDate, shiftsFromLatest.First().StartTime);
                var endTime = CombineDateTimeWithDateWithTimeSpan(shiftDate, shiftsFromLatest.First().EndTime);
                return new ShiftModel(startTime, endTime);
            }


            //same day - return last shift from accurent shift with same date
            int index = shiftsFromLatest.FindIndex(x => x.StartTime == lastOccupiedShift.ShiftStart.TimeOfDay);
            if(index >= 0)
            {
                var newShift = shiftsFromLatest[++index];
                var startTime = CombineDateTimeWithDateWithTimeSpan(lastOccupiedShift.ShiftStart, newShift.StartTime);
                var endTime = CombineDateTimeWithDateWithTimeSpan(lastOccupiedShift.ShiftEnd, newShift.EndTime);
                return new ShiftModel(startTime, endTime);
            }

            //same day - shift is not finished
            if(index == -1)
            {
                var unfinishedShift = shiftsFromLatest.First(x => x.EndTime == lastOccupiedShift.ShiftEnd.TimeOfDay);
                var startTime = CombineDateTimeWithDateWithTimeSpan(lastOccupiedShift.ShiftStart, unfinishedShift.StartTime);
                return new ShiftModel(startTime, lastOccupiedShift.ShiftStart);
            }
            throw new NotImplementedException();
        }

        private DateTime GetAviableDate(List<DayOfWeek> workWeekDays, DateTime actualDay)
        {
            for (int i = 0; i < 7; i++)
            {
                if (workWeekDays.Any(x => x == actualDay.DayOfWeek))
                {
                    return actualDay;
                }
                actualDay = actualDay.AddDays(-1);
            }
            throw new NotImplementedException();
        }

        public async Task<ProductionSheduleReportsModel> GetReports()
        {
            return new ProductionSheduleReportsModel()
            {
                Reports = await _mrpRepository.GetProductionSheduleReports()
            };
        }
    }
}
