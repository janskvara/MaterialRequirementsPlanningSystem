using BusinessLogic.CapacityPlanningService;
using BusinessLogic.FactoryModelsService;
using BusinessLogic.Models;
using DataAcess.Entities;
using DataAcess.Repositories;

namespace BusinessLogic.MRPService
{
    public interface IProductionShedule
    {
        public Task Plan(OrderPlanRequest productionPlan);
        public Task<ProductionSheduleReportsModel> GetReports();
    }

    public class ProductionShedule: IProductionShedule
    {
        private readonly IProductionCapacityCalculator _productionCapacityCalculator;
        private readonly IFactoryModelQuery _factoryModelQuery;
        private readonly IMRPRepository _mrpRepository;
        private readonly IWarehouseRepository _warehouseRepository;

        public ProductionShedule(IProductionCapacityCalculator productionCapacityCalculator, IFactoryModelQuery factoryModelQuery, 
            IMRPRepository mrpRepository, IWarehouseRepository warehouseRepository)
        {
            _productionCapacityCalculator= productionCapacityCalculator;
            _factoryModelQuery= factoryModelQuery;
            _mrpRepository= mrpRepository;
            _warehouseRepository= warehouseRepository;
        }

        public async Task Plan(OrderPlanRequest productionPlan)
        {
            var logs = new List<string>();
            var events = new List<EventEntity>();
            var productionEvents = new List<ManufacturesEventModel>();
            var orderedOrders = productionPlan.Orders.OrderBy(x => x.ExportDate).Reverse().ToList();
            var occupiedDate = orderedOrders.First().ExportDate;

            foreach (var order in orderedOrders)
            {
                var model = await _factoryModelQuery.GetFactoryModelAsync(order.ModelId);
                var numberOfShifts = await _productionCapacityCalculator.CalculateNumberOfShifts(model.RouteSheet.Id, order.Quantity);

                var finishDay = order.ExportDate.AddDays(-1);//finish day is also working day (one shift)
                if(finishDay >= occupiedDate)
                    finishDay = occupiedDate.AddDays(-1);

                var productionStartDate = finishDay.AddDays(-numberOfShifts + 1);
                if (productionStartDate < productionPlan.FromDate)
                {
                    logs.Add($"Kapacitne planovanie obmedzuje vykonat objednavku:{order.OrderId}. Zaciatok vyroby: {productionStartDate}" +
                        $"by bol pred datumom zacatia planu: {productionPlan.FromDate}.");
                    continue;
                }

                var productionEvent = new ManufacturesEventModel(order.OrderId, model.Name, productionStartDate, finishDay);
                productionEvents.Add(productionEvent);
                events.Add(productionEvent.Event);
                occupiedDate = productionStartDate;

                var exportEvent = new ExportEventModel(order.OrderId, model.Name, order.ExportDate);
                events.Add(exportEvent.Event);
            }

            var orderedProductionEvents = productionEvents.OrderBy(x => x.Event.StartTime).ToList();
            var changesInWareHouse = new ChangesInWarehouseModel();
            var orders = new List<OrderGoodsEntities>();
            foreach(var productionEvent in orderedProductionEvents)
            {
                var order = productionPlan.Orders.First(x => x.OrderId == productionEvent.OrderId);
                var model = await _factoryModelQuery.GetFactoryModelAsync(order.ModelId);

                var wholeBill = model.SummarryMaterialList.Select(s => new SummarryMaterialOfBill { 
                    PartNumber = s.PartNumber,
                    Quantity = s.Quantity * order.Quantity,
                    ModelType = s.ModelType,
                    Description= s.Description,
                    Unit= s.Unit
                }).ToList();
                var goodsList = (await _warehouseRepository.GetAllGoodsInProductionWarehouse())
                    .FindAll(s => wholeBill.Any(x => x.PartNumber.Equals(s.PartNumber)));

                //inicializacia zmien vo warehouse -> zaciname aktualnym stavov na sklade
                foreach(var goods in goodsList)
                {
                    changesInWareHouse.AddGoodsChange(goods.PartNumber, productionPlan.FromDate, 0, goods.ActualCapacity);
                }

                var orderGoodsEntity = new OrderGoodsEntities(productionEvent.Event.StartTime.AddDays(-1));
                foreach(var component in wholeBill)
                {
                    var componentInWarehouse = goodsList.First(x => x.PartNumber.Equals(component.PartNumber));
                    var lastChangeOfTheComponent = changesInWareHouse.ChangesOfProducts[component.PartNumber].Last();
                    var newCapacity = lastChangeOfTheComponent.NewCapacity - component.Quantity;

                    //ak by nebolo dostatok vyrobkov po vyrobe -> znamena ze este pred vyrobov potrebujeme vytovrit objednavku
                    //treba porozmyslat co s maxCapacity ak sa prekroci pre danu objednavku mu sa objednavka rozdelit na viac objednavok
                    if(newCapacity < componentInWarehouse.MinimumThresholdCapacity)
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
                Description = logs
            };
            await _mrpRepository.SaveProductionSheduleReport(report);
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
