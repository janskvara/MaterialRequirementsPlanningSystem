using DataAcess.Entities.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAcess.Entities
{
    public class GoodsEntity
    {
        [BsonId]
        public string PartNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ModelType ModelType { get; set; }
        public string Unit { get; set; } = string.Empty;
        public double MaxCapacityOfProduct { get; set; }
        public double ActualCapacity { get; set; }
        public double MinimumThresholdCapacity { get; set; }
    }
}
