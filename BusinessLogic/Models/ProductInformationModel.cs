
namespace BusinessLogic.Models
{
    public class ProductInformationModel
    {
        public string PartNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int RouteSheetId { get; set; }
        public string ModelType { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public RouteSheetModel RouteSheet { get; set; } = new();
        public List<ProductRequirementWithSequenceModel> BillOfMaterials { get; set; } = new();
        public List<ProductRequirementModel> SummaryBillOfMaterials { get; set; } = new();
    }
}
