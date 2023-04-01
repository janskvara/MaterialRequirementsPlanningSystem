using BusinessLogic.CapacityPlanningService;
using BusinessLogic.Models;
using DataAcess.Entities;
using DataAcess.Repositories;

namespace BusinessLogic.FactoryModelsService
{
    public interface IFactoryModelQuery
    {
        public Task<FactoryModelsResponseModel> GetFactoryModelsAsync();
        public Task<FactoryModelResponse> GetFactoryModelAsync(string modelId);
    }

    public class FactoryModelQuery : IFactoryModelQuery
    {
        private readonly IFactoryModelRepository _factoryModelRepository;
        private readonly ICapacityPlanningQuery _capacityPlanningQuery;
        public FactoryModelQuery(IFactoryModelRepository factoryModelRepository, ICapacityPlanningQuery capacityPlanningQuery)
        {
            _factoryModelRepository = factoryModelRepository;
            _capacityPlanningQuery = capacityPlanningQuery;
        }

        public async Task<FactoryModelResponse> GetFactoryModelAsync(string modelId)
        {
            var model = (await _factoryModelRepository.GetFactoryModels()).First(x => x.Id == modelId);
            var billOfMaterials = await _factoryModelRepository.GetBillOfMaterial(model.BillOfMaterialId);
            var routeSheet = await _capacityPlanningQuery.GetRouteSheetAsync(model.RouteSheetId);
            return new FactoryModelResponse()
            {
                Id = model.Id,
                Name = model.Name,
                ModelType = model.ModelType.ToString(),
                RouteSheet = routeSheet,
                MaterialList = billOfMaterials.MaterialList.Select(x => new MaterialOfBillModel()
                {
                    Order = x.Order,
                    PartNumber = x.PartNumber,
                    Description = x.Description,
                    ModelType = x.ModelType.ToString(),
                    Quantity = x.Quantity,
                    Unit = x.Unit
                }).ToList(),
                SummarryMaterialList = GetSummarryMaterialsOfBillAsync(billOfMaterials.MaterialList)
            };
        }

        public async Task<FactoryModelsResponseModel> GetFactoryModelsAsync()
        {
            var models = await _factoryModelRepository.GetFactoryModels();
            var response = new FactoryModelsResponseModel();

            foreach (var model in models)
            {
                var billOfMaterials = await _factoryModelRepository.GetBillOfMaterial(model.BillOfMaterialId);
                var routeSheet = await _capacityPlanningQuery.GetRouteSheetAsync(model.RouteSheetId);
                var responseModel = new FactoryModelResponse()
                {
                    Id = model.Id,
                    Name = model.Name,
                    ModelType = model.ModelType.ToString(),
                    RouteSheet = routeSheet,
                    MaterialList = billOfMaterials.MaterialList.Select(x => new MaterialOfBillModel()
                    {
                        Order = x.Order,
                        PartNumber = x.PartNumber,
                        Description = x.Description,
                        ModelType = x.ModelType.ToString(),
                        Quantity = x.Quantity,
                        Unit = x.Unit
                    }).ToList(),
                    SummarryMaterialList = GetSummarryMaterialsOfBillAsync(billOfMaterials.MaterialList)
                };
                response.FactoryModels.Add(model.Name, responseModel);
            }
            return response;
        }

        private List<SummarryMaterialOfBill> GetSummarryMaterialsOfBillAsync(List<MaterialOfBill> billOfMaterials)
        {
            var combinedElements = billOfMaterials.GroupBy(x => x.PartNumber);
            //ak budu aj subkomponenty treba popremyslat nad nejakym typom rekurzie
            var summaryMaterialsOfBills = combinedElements.Select(x => new SummarryMaterialOfBill()
            {
                PartNumber = x.First().PartNumber,
                Description = x.First().Description,
                ModelType = x.First().ModelType,
                Quantity = x.Sum(z => z.Quantity),
                Unit = x.First().Unit,
            });
            return summaryMaterialsOfBills.ToList();
        }
    }
}
