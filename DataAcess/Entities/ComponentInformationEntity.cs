using DataAcess.Entities.Enums;
using DataAcess.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAcess.Entities
{
    public class ComponentInformationEntity
    {
        [BsonId]
        public string PartNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ModelType ModelType { get; set; }
        public string Unit { get; set; } = string.Empty;
    }

    public class ProductInformationEntity: ComponentInformationEntity
    {
        public int RouteSheetId { get; set; }
        public List<ProductRequirementWithSequence> BillOfMaterials { get; set; } = new();
    }
}
