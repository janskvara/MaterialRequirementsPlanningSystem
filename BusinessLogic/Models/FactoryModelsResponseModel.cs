namespace BusinessLogic.Models
{
    public class FactoryModelResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ModelType { get; set; } = string.Empty;
        public RouteSheetResponseModel RouteSheet { get; set; } = new();
        public List<MaterialOfBillModel> MaterialList { get; set; } = new();
        public List<ProductRequirementModel> SummarryMaterialList { get; set; } = new();
    }

    public class FactoryModelsResponseModel
    {
       public  Dictionary<string,FactoryModelResponse> FactoryModels { get; set; } = new();
    }
}
