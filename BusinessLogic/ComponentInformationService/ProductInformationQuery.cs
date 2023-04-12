using BusinessLogic.DepartmentService;
using BusinessLogic.Models;
using DataAcess.Models;
using DataAcess.Repositories;

namespace BusinessLogic.ComponentInformationService
{
    public interface IProductInformationQuery
    {
        public Task<List<ProductInformationModel>> GetProductsInformation(int departmentId);
        public Task<ProductInformationModel> GetProductInformation(string partNumber);
    }

    public class ProductInformationQuery: IProductInformationQuery
    {
        private readonly IComponentInformationRepository _componentInformationRepository;
        private readonly IRouteSheetQuery _routeSheetQuery;
        public ProductInformationQuery(IComponentInformationRepository componentInformationRepository, IRouteSheetQuery routeSheetQuery)
        {
            _componentInformationRepository = componentInformationRepository;
            _routeSheetQuery = routeSheetQuery;
        }

        public async Task<List<ProductInformationModel>> GetProductsInformation(int departmentId)
        {
            List<ProductInformationModel> productInformation = new();
            var routeSheets = await _routeSheetQuery.GetRouteSheetsAsync(departmentId);
            var products = await _componentInformationRepository.GetProductsInformation(routeSheets.Select(x => x.Id).ToList());
            foreach (var product in products)
            {
                var billOfMaterials = await GetBillOfMaterialsModel(product.BillOfMaterials);
                var summaryBillOfMaterials = GetSummarryMaterialsOfBillAsync(billOfMaterials);
                var routeSheet = routeSheets.First(x => x.Id == product.RouteSheetId);
                productInformation.Add(new ProductInformationModel
                {
                    PartNumber = product.PartNumber,
                    Description = product.Description,
                    RouteSheetId = product.RouteSheetId,
                    ModelType = product.ModelType.ToString(),
                    Unit = product.Unit,
                    RouteSheet = routeSheet,
                    BillOfMaterials = billOfMaterials,
                    SummaryBillOfMaterials = summaryBillOfMaterials
                });
            }
            return productInformation;
        }

        public async Task<ProductInformationModel> GetProductInformation(string partNumber)
        {
            var product = await _componentInformationRepository.GetProductInformation(partNumber);

            var billOfMaterials = await GetBillOfMaterialsModel(product.BillOfMaterials);
            var summaryBillOfMaterials = GetSummarryMaterialsOfBillAsync(billOfMaterials);
            var routeSheet = await _routeSheetQuery.GetRouteSheetAsync(product.RouteSheetId);
            return new ProductInformationModel
            {
                PartNumber = product.PartNumber,
                Description = product.Description,
                RouteSheetId = product.RouteSheetId,
                ModelType = product.ModelType.ToString(),
                Unit = product.Unit,
                RouteSheet = routeSheet,
                BillOfMaterials = billOfMaterials,
                SummaryBillOfMaterials = summaryBillOfMaterials
            };
        }

        private List<ProductRequirementModel> GetSummarryMaterialsOfBillAsync(List<ProductRequirementWithSequenceModel> billOfMaterials)
        {
            var combinedElements = billOfMaterials.GroupBy(x => x.PartNumber);
            //ak budu aj subkomponenty treba popremyslat nad nejakym typom rekurzie
            var summaryMaterialsOfBills = combinedElements.Select(x => new ProductRequirementModel()
            {
                PartNumber = x.First().PartNumber,
                Description= x.First().Description,
                ModelType = x.First().ModelType,
                Quantity = x.Sum(z => z.Quantity),
                Unit= x.First().Unit,
            });
            return summaryMaterialsOfBills.ToList();
        }

        private async Task<List<ProductRequirementWithSequenceModel>> GetBillOfMaterialsModel(List<ProductRequirementWithSequence> productRequirements)
        {
            List<ProductRequirementWithSequenceModel> billOfMaterials = new ();
            foreach (var productRequirement in productRequirements)
            {
                var component = await _componentInformationRepository.GetComponentInformation(productRequirement.PartNumber);
                billOfMaterials.Add(new ProductRequirementWithSequenceModel
                {
                    PartNumber = productRequirement.PartNumber,
                    Description = component.Description,
                    Quantity = productRequirement.Quantity,
                    ModelType = component.ModelType,
                    Unit = component.Unit,
                    Sequence = productRequirement.Sequence,
                });
            }
            return billOfMaterials;
        }

    }
}
