using DataAcess.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAcess.Entities
{
    public class ShiftSummaryEntity
    {
        [BsonId]
        public string Id { get; set; } = string.Empty;
        public string ReportId { get; set; } = string.Empty;
        public int OrderId { get; set; }
        public double Quantity { get; set; }
        public DateTime ShiftStart { get; set; }
        public DateTime ShiftEnd { get; set; }
        public string NameOfManufacturedModel { get; set; } = string.Empty;
        public List<ProductRequirement> BillOfMaterials { get; set; } = new();
    }
}
